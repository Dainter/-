using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    public class ProcedureTask:IfTask
    {
        Task task;
        string strDescription;
        //CurrentStep[1:1]
        ProcedureStep curStep;

        public string Name
        {
            get
            {
                return task.Name;
            }
        }
        public string Type
        {
            get { return this.GetType().Name ; }
        }
        public TaskType BusinessType
        {
            get { return this.task.BusinessType; }
            set { this.task.BusinessType = value; }
        }
        public bool IsBindingProcedure
        {
            get
            {
                if(task.BusinessType == null)
                {
                    return false;
                }
                return this.task.IsBindingProcedure;
            }
        }
        public ProcedureStep CurrentStep
        {
            get { return this.curStep; }
            set { this.curStep = value; }
        }
        public IfUser Submitter
        {
            get { return this.task.Submitter; }
            set { this.task.Submitter = value; }
        }
        public IfUser Handler
        {
            get { return this.task.Handler; }
            set { this.task.Handler = value; }
        }
        public DateTime StartTime
        {
            get
            {
                return task.StartTime;
            }
        }
        public DateTime DeadLine
        {
            get
            {
                return task.DeadLine;
            }
        }
        public DateTime CompletedTime
        {
            get { return this.task.CompletedTime; }
        }
        public QLevel QLevel
        {
            get { return this.task.QLevel; }
            set { this.task.QLevel = value; }
        }
        public string Status
        {
            get { return this.task.Status; }
            set { this.task.Status = value; }
        }
        public string DelayReason
        {
            get { return this.task.DelayReason; }
            set { this.task.DelayReason = value; }
        }
        public string Description
        {
            get { return strDescription; }
        }

        public ProcedureTask(string sName, DateTime dStart, DateTime dDead, string sDescription)
        {
            this.task = new Task(sName,
                                        dStart, 
                                        dDead);
            this.curStep = null;
            //流程任务进入等待区，等待后台调度器调度
            this.Status = "Wait";
            this.strDescription = sDescription;
        }

        public ProcedureTask(XmlElement modelPayload)
        {
            this.task = new Task(modelPayload);
            this.curStep = null;
            this.strDescription = Utility.GetText(modelPayload, "Description");
        }

        public void UpdateRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            Record record;
            //TaskType[1:1]
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "SetType");
            this.task.BusinessType  = dataset.GetTypeItem(record.Name);
            //Sumitter[1:1]
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "Submitter");
            this.task.Submitter = dataset.GetUserItem(record.Name);
            //Handler[1:1]
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "Handler");
            this.task.Handler = dataset.GetUserItem(record.Name);
            //Priority[1:1]
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "SetPriority");
            this.task.QLevel = dataset.GetQlevelItem(record.Name);
            //CurrentStep[1:1]
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "CurrentStep");
            this.curStep = dataset.GetStepItem(record.Name);
        }

        public XmlElement XMLSerialize(XmlElement BusinessPayload = null)
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, start_txt, dead_txt, comp_txt, status_txt, reason_txt, desc_txt;
            XmlElement name_xml, start_xml, dead_xml, comp_xml, status_xml, reason_xml, desc_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            start_xml = doc.CreateElement("StartTime");
            dead_xml = doc.CreateElement("DeadLine");
            comp_xml = doc.CreateElement("CompletedTime");
            status_xml = doc.CreateElement("Status");
            reason_xml = doc.CreateElement("DelayReason");
            //private
            desc_xml = doc.CreateElement("Description");

            name_txt = doc.CreateTextNode(this.Name);
            start_txt = doc.CreateTextNode(this.StartTime.ToString());
            dead_txt = doc.CreateTextNode(this.DeadLine.ToString());
            comp_txt = doc.CreateTextNode(this.CompletedTime.ToString());
            status_txt = doc.CreateTextNode(this.Status);
            reason_txt = doc.CreateTextNode(this.DelayReason);
            desc_txt = doc.CreateTextNode(this.Description);

            name_xml.AppendChild(name_txt);
            start_xml.AppendChild(start_txt);
            dead_xml.AppendChild(dead_txt);
            comp_xml.AppendChild(comp_txt);
            status_xml.AppendChild(status_txt);
            reason_xml.AppendChild(reason_txt);
            desc_xml.AppendChild(desc_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(start_xml);
            modelPayload.AppendChild(dead_xml);
            modelPayload.AppendChild(comp_xml);
            modelPayload.AppendChild(status_xml);
            modelPayload.AppendChild(reason_xml);
            modelPayload.AppendChild(desc_xml);
            if(BusinessPayload != null)
            {
                modelPayload.AppendChild(doc.ImportNode(BusinessPayload, true));
            }

            return modelPayload;

        }

        public override string ToString()
        {
            return Name;
        }
    }

}
