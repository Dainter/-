using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    public class TaskType
    {
        //Name = strBussinessType
        string strName;
        //string strBussinessType;
        string strDescription;
        int intPriority;
        //Procedure(if IsDirectly = true then null)[1:1]
        Procedure procedure;
        
        public string Name
        {
            get{ return strName; }
        }

        public string Type
        {
            get { return this.GetType().Name; }
        }

        public string Description
        {
            get
            { return strDescription; }
        }

        public bool IsUseProcedure
        {
            get
            {
                if(procedure == null)
                {
                    return false;
                }
                return true;
            }
        }

        public int Priority
        {
            get { return intPriority; }
            set { intPriority = value; }
        }

        public Procedure BindingProcedure
        {
            get { return procedure; }
            set { procedure = value; }
        }

        public TaskType(string sName, int iPriority = 50, string sDescription = "")
        {
            this.strName = sName;
            
            this.intPriority = iPriority;
            if (iPriority > 100 || iPriority <=0)
            {
                this.intPriority = 50;
            }
            this.strDescription = sDescription;
        }

        public TaskType(XmlElement ModelPayload)
        {
            this.strName = Utility.GetText(ModelPayload, "Name");
            this.intPriority = Convert.ToInt32(Utility.GetText(ModelPayload, "Priority"));
            this.strDescription = Utility.GetText(ModelPayload, "Description");
        }

        public void ExtractRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            //Procedure
            Record record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "Assign");
            this.procedure = dataset.GetProcedureItem(record.Name);
        }

        public void StoreRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            RelationShip newRelation;
            if(this.IsUseProcedure == false)
            {
                return;
            }
            //Procedure
            newRelation = new RelationShip(this.Name, this.Type, this.procedure.Name, this.procedure.Type, "Assign", "1");
            DataReader.InsertRelationShip(newRelation);
        }

        public XmlElement XMLSerialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, desc_txt, prio_txt;
            XmlElement name_xml, desc_xml, prio_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            prio_xml = doc.CreateElement("Priority");
            desc_xml = doc.CreateElement("Description");

            name_txt = doc.CreateTextNode(this.Name);
            prio_txt = doc.CreateTextNode(this.Priority.ToString());
            desc_txt = doc.CreateTextNode(this.Description);

            name_xml.AppendChild(name_txt);
            prio_xml.AppendChild(prio_txt);
            desc_xml.AppendChild(desc_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(prio_xml);
            modelPayload.AppendChild(desc_xml);

            return modelPayload;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
