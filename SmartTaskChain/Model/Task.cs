using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphDB.Core;

namespace SmartTaskChain.Model
{
    class Task:IfTask
    {
        //Name
        string strName;
        //TaskType[1:1]
        TaskType taskType;
        //Sumitter[1:1]
        //Handler[1:1]
        //CurrentStep[1:1]
        ProcedureStep currentStep;
        //StartTime
        DateTime datStartTime;
        //DeadLine
        DateTime datDeadLine;
        //CompleteTime
        DateTime datCompletedTime;
        //Priority[1:1]
        QLevel eQlevel;
        //TaskStatus
        TaskStatus.EnumTaskStatus taskStatus;
        //DelayReason(if TaskStatus == Delay then not null)
        string strDelayReason;
        
        public string Name
        {
            get { return strName; }
        }

        public string Type
        {
            get { return this.GetType().Name; }
        }

        public string BusinessType
        {
            get { return this.taskType.Name; }
        }

        public DateTime StartTime
        {
            get { return datStartTime; }
        }

        public DateTime DeadLine
        {
            get { return datDeadLine; }
        }

        public QLevel QLevel
        {
            get { return eQlevel; }
        }

        public string Status
        {
            get { return TaskStatus.ToString(taskStatus); }
            set { taskStatus = TaskStatus.ToEnum(value); }
        }

        public Task(string sName, string sBType, DateTime dStart, DateTime dDead, bool IsImportant = false, bool IsEmergency = false)
        {
            this.strName = sName;
            this.taskType = TaskTypeFactory.GetFactory().GetTaskType(sBType);
            this.datStartTime = dStart;
            this.datDeadLine = dDead;
            this.eQlevel = QLevelFactory.GetFactory().GetQlevel(IsImportant, IsEmergency);
            this.taskStatus = TaskStatus.EnumTaskStatus.Created;
        }

        public Task(XmlElement ModelPayload)
        {
            this.strName = GetText(ModelPayload, "Name");
            this.taskType = TaskTypeFactory.GetFactory().GetTaskType(GetText(ModelPayload, "BusinessType"));
            this.datStartTime = Convert.ToDateTime(GetText(ModelPayload, "StartTime"));
            this.datDeadLine = Convert.ToDateTime(GetText(ModelPayload, "DeadLine"));
            this.eQlevel = QLevelFactory.GetFactory().GetQlevel(GetText(ModelPayload, "QLevel"));
            this.taskStatus = TaskStatus.ToEnum(GetText(ModelPayload, "Status"));
        }

        //工具函数，从xml节点中读取某个标签的InnerText
        string GetText(XmlElement curNode, string sLabel)
        {
            if (curNode == null)
            {
                return "";
            }
            //遍历子节点列表
            foreach (XmlElement xNode in curNode.ChildNodes)
            {
                if (xNode.Name == sLabel)
                {//查找和指定内容相同的标签，返回其Innner Text
                    return xNode.InnerText;
                }
            }
            return "";
        }

        public XmlElement XMLSerialize(XmlElement BusinessPayload)
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, btype_txt, start_txt, dead_txt, qlevel_txt, status_txt;
            XmlElement name_xml, btype_xml, start_xml, dead_xml, qlevel_xml, status_xml, modelPayload;

            modelPayload = doc.CreateElement("ModelPayload");
            name_xml = doc.CreateElement("Name");
            btype_xml = doc.CreateElement("BusinessType");
            start_xml = doc.CreateElement("StartTime");
            dead_xml = doc.CreateElement("DeadLine");
            qlevel_xml = doc.CreateElement("QLevel");
            status_xml = doc.CreateElement("Status");

            name_txt = doc.CreateTextNode(this.Name);
            btype_txt = doc.CreateTextNode(this.BusinessType);
            start_txt = doc.CreateTextNode(this.StartTime.ToString());
            dead_txt = doc.CreateTextNode(this.DeadLine.ToString());
            qlevel_txt = doc.CreateTextNode(this.QLevel.Name);
            status_txt = doc.CreateTextNode(this.Status);

            name_xml.AppendChild(name_txt);
            btype_xml.AppendChild(btype_txt);
            start_xml.AppendChild(start_txt);
            dead_xml.AppendChild(dead_txt);
            qlevel_xml.AppendChild(qlevel_txt);
            status_xml.AppendChild(status_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(btype_xml);
            modelPayload.AppendChild(start_xml);
            modelPayload.AppendChild(dead_xml);
            modelPayload.AppendChild(qlevel_xml);
            modelPayload.AppendChild(status_xml);
            modelPayload.AppendChild(doc.ImportNode(BusinessPayload, true));

            return modelPayload;
        }

    }
}
