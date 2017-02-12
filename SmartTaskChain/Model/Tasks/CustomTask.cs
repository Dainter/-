using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    public class CustomTask : IfTask
    {
        Task task;
        string strDescription;
        
        public string Name
        {
            get
            {
                return task.Name;
            }
        }
        public string Type
        {
            get { return this.GetType().Name; }
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
                if (task.BusinessType == null)
                {
                    return false;
                }
                return this.task.IsBindingProcedure;
            }
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
        public string Description
        {
            get { return strDescription; }
            set { strDescription = value; }
        }
        public double Priority
        {
            get { return this.task.Priority; }
            set { this.task.Priority = value; }
        }

        public CustomTask(string sName, DateTime dStart, DateTime dDead, string sDescription)
        {
            this.task = new Task(sName,
                                        dStart,
                                        dDead);
            this.strDescription = sDescription;
        }

        public CustomTask(XmlElement modelPayload)
        {
            this.task = new Task(modelPayload);
            this.strDescription = Utility.GetText(Utility.GetNode(modelPayload, "BussinessPayload"), "Description");
        }

        public void UpdateRealtion(TaskType objType, IfUser objSub, IfUser objHand, QLevel objLevel)
        {
            this.task.BusinessType = objType;
            this.task.Submitter = objSub;
            this.task.Handler = objHand;
            this.task.QLevel = objLevel;
            UpdatePriority();
        }

        public void ExtractRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            Record record;
            //TaskType[1:1]
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "SetType");
            this.task.BusinessType = dataset.GetTypeItem(record.Name);
            //Sumitter[1:1]
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "Submitter");
            this.task.Submitter = dataset.GetUserItem(record.Name);
            //Handler[1:1]
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "Handler");
            this.task.Handler = dataset.GetUserItem(record.Name);
            //Priority[1:1]
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "SetPriority");
            this.task.QLevel = dataset.GetQlevelItem(record.Name);
            UpdatePriority();
        }

        public void StoreRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            RelationShip newRelation;
            if (this.task.BusinessType != null)
            {
                //TaskType[1:1]
                newRelation = new RelationShip(this.Name, this.Type, this.task.BusinessType.Name, this.task.BusinessType.Type, "SetType", "1");
                DataReader.InsertRelationShip(newRelation);
            }
            if (this.task.Submitter != null)
            {
                //Sumitter[1:1]
                newRelation = new RelationShip(this.Name, this.Type, this.task.Submitter.Name, this.task.Submitter.Type, "Submitter", "1");
                DataReader.InsertRelationShip(newRelation);
            }
            if (this.task.Handler != null)
            {
                //Handler[1:1]
                newRelation = new RelationShip(this.Name, this.Type, this.task.Handler.Name, this.task.Handler.Type, "Handler", "1");
                DataReader.InsertRelationShip(newRelation);
            }
            if (this.task.QLevel != null)
            {
                //Priority[1:1]
                newRelation = new RelationShip(this.Name, this.Type, this.task.QLevel.Name, this.task.QLevel.Type, "SetPriority", "1");
                DataReader.InsertRelationShip(newRelation);
            }
        }

        public void UpdatePriority()
        {
            if(BusinessType == null || QLevel == null)
            {
                return;
            }
            task.UpdatePriority();
        }

        public XmlElement XMLSerialize(XmlElement BusinessPayload = null)
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, start_txt, dead_txt, status_txt, desc_txt;
            XmlElement name_xml, start_xml, dead_xml, status_xml, desc_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            start_xml = doc.CreateElement("StartTime");
            dead_xml = doc.CreateElement("DeadLine");
            status_xml = doc.CreateElement("Status");
            //private
            desc_xml = doc.CreateElement("Description");

            name_txt = doc.CreateTextNode(this.Name);
            start_txt = doc.CreateTextNode(this.StartTime.ToString());
            dead_txt = doc.CreateTextNode(this.DeadLine.ToString());
            status_txt = doc.CreateTextNode(this.Status);
            desc_txt = doc.CreateTextNode(this.Description);

            name_xml.AppendChild(name_txt);
            start_xml.AppendChild(start_txt);
            dead_xml.AppendChild(dead_txt);
            status_xml.AppendChild(status_txt);
            desc_xml.AppendChild(desc_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(start_xml);
            modelPayload.AppendChild(dead_xml);
            modelPayload.AppendChild(status_xml);
            modelPayload.AppendChild(desc_xml);
            if (BusinessPayload != null)
            {
                modelPayload.AppendChild(doc.ImportNode(BusinessPayload, true));
            }

            return modelPayload;
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(IfTask other)
        {
            // A null value means that this object is greater.
            if (other == null)
            {
                return 1;
            }
            if (this.Priority >= 0)
            {
                if (other.Priority >= 0)
                {
                    return 0-this.Priority.CompareTo(other.Priority);
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (other.Priority >= 0)
                {
                    return -1;
                }
                else
                {
                    return 0-this.Priority.CompareTo(other.Priority);
                }
            }
        }
    }
}
