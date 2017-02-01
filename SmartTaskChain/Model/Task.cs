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
        IfUser usrSubmitter;
        //CurrentStep[1:1]
        ProcedureStep curStep;
        //Handler[1:1]
        IfUser usrHandler;
        //StartTime
        DateTime datStartTime;
        //DeadLine
        DateTime datDeadLine;
        //CompleteTime(if TaskStatus == Completed then not null)
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
        public TaskType BusinessType
        {
            get { return this.taskType; }
        }
        public IfUser Submitter
        {
            get { return usrSubmitter; }
            set { usrSubmitter = value; }
        }
        public ProcedureStep CurrentStep
        {
            get { return curStep; }
            set { curStep = value; }
        }
        public IfUser Handler
        {
            get { return usrHandler; }
            set { usrHandler = value; }
        }
        public DateTime StartTime
        {
            get { return datStartTime; }
        }
        public DateTime DeadLine
        {
            get { return datDeadLine; }
        }
        public DateTime CompletedTime
        {
            get { return datCompletedTime; }
        }
        public QLevel QLevel
        {
            get { return eQlevel; }
            set { eQlevel = value; }
        }
        public string Status
        {
            get { return TaskStatus.ToString(taskStatus); }
            set { taskStatus = TaskStatus.ToEnum(value); }
        }
        public string DelayReason
        {
            get { return strDelayReason; }
            set { strDelayReason = value; }
        }

        public Task(string sName, DateTime dStart, DateTime dDead)
        {
            this.strName = sName;
            this.taskType = null;
            this.usrSubmitter = null;
            this.curStep = null;
            this.usrHandler = null;
            this.datStartTime = dStart;
            this.datDeadLine = dDead;
            this.datCompletedTime = new DateTime(0);
            this.eQlevel = null;
            this.taskStatus = TaskStatus.EnumTaskStatus.Process;
            this.strDelayReason = "";
        }

        public Task(XmlElement ModelPayload)
        {
            this.strName = GetText(ModelPayload, "Name");
            this.taskType = null;
            this.usrSubmitter = null;
            this.curStep = null;
            this.usrHandler = null;
            this.datStartTime = Convert.ToDateTime(GetText(ModelPayload, "StartTime"));
            this.datDeadLine = Convert.ToDateTime(GetText(ModelPayload, "DeadLine"));
            this.datCompletedTime = Convert.ToDateTime(GetText(ModelPayload, "CompletedTime"));
            this.eQlevel = null;
            this.taskStatus = TaskStatus.ToEnum(GetText(ModelPayload, "Status"));
            this.strDelayReason = GetText(ModelPayload, "DelayReason"); ;
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
            XmlText name_txt, start_txt, dead_txt, comp_txt, status_txt, reason_txt;
            XmlElement name_xml, start_xml, dead_xml, comp_xml, status_xml, reason_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            start_xml = doc.CreateElement("StartTime");
            dead_xml = doc.CreateElement("DeadLine");
            comp_xml = doc.CreateElement("CompletedTime");
            status_xml = doc.CreateElement("Status");
            reason_xml = doc.CreateElement("DelayReason");

            name_txt = doc.CreateTextNode(this.Name);
            start_txt = doc.CreateTextNode(this.StartTime.ToString());
            dead_txt = doc.CreateTextNode(this.DeadLine.ToString());
            comp_txt = doc.CreateTextNode(this.CompletedTime.ToString());
            status_txt = doc.CreateTextNode(this.Status);
            reason_txt = doc.CreateTextNode(this.DelayReason);

            name_xml.AppendChild(name_txt);
            start_xml.AppendChild(start_txt);
            dead_xml.AppendChild(dead_txt);
            comp_xml.AppendChild(comp_txt);
            status_xml.AppendChild(status_txt);
            reason_xml.AppendChild(reason_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(start_xml);
            modelPayload.AppendChild(dead_xml);
            modelPayload.AppendChild(comp_xml);
            modelPayload.AppendChild(status_xml);
            modelPayload.AppendChild(reason_xml);
            modelPayload.AppendChild(doc.ImportNode(BusinessPayload, true));

            return modelPayload;
        }

    }
}
