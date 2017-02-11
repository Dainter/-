using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    public class Procedure
    {
        string strName;
        string strDescription;
        //TaskType[1:1]
        TaskType taskType;
        //ProcedureSteps()[1:n]
        List<ProcedureStep> procedureSteps;

        public string Name
        {
            get
            { return strName; }
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

        public bool IsBindingType
        {
            get
            {
                if (taskType == null)
                {
                    return false;
                }
                return true;
            }
        }

        public TaskType BindingType
        {
            get { return taskType; }
            set { taskType = value; }
        }

        public List<ProcedureStep> ProcedureSteps
        {
            get { return procedureSteps; }
            set { procedureSteps = value; }
        }

        public Procedure(string sName, string sDescription="")
        {
            this.strName = sName;
            this.strDescription = sDescription;
            this.procedureSteps = new List<ProcedureStep>();
        }

        public Procedure(XmlElement ModelPayload)
        {
            this.strName = Utility.GetText(ModelPayload, "Name");
            this.strDescription = Utility.GetText(ModelPayload, "Description");
            this.procedureSteps = new List<ProcedureStep>();
        }

        public void ExtractRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            //TaskType
            Record record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "Binding");
            this.taskType = dataset.GetTypeItem(record.Name);
            //ProcedureSteps
            this.procedureSteps.Clear();
            List<string> steps = DataReader.GetDNodesBySNodeandEdgeType(this.Name, this.Type, "Include");
            foreach(string stepname in steps)
            {
                this.procedureSteps.Add(dataset.GetStepItem(stepname));
            }
            procedureSteps.Sort();
        }

        public void StoreRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            RelationShip newRelation;
            if (this.taskType != null)
            {
                //TaskType
                newRelation = new RelationShip(this.Name, this.Type, this.taskType.Name, this.taskType.Type, "Binding", "1");
                DataReader.InsertRelationShip(newRelation);
            }
            if(this.procedureSteps.Count > 0 )
            {
                //ProcedureSteps
                foreach (ProcedureStep step in procedureSteps)
                {
                    newRelation = new RelationShip(this.Name, this.Type, step.Name, step.Type, "Include", "1");
                    DataReader.InsertRelationShip(newRelation);
                }
            }
        }

        public XmlElement XMLSerialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, desc_txt;
            XmlElement name_xml, desc_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            desc_xml = doc.CreateElement("Description");

            name_txt = doc.CreateTextNode(this.Name);
            desc_txt = doc.CreateTextNode(this.Description);

            name_xml.AppendChild(name_txt);
            desc_xml.AppendChild(desc_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(desc_xml);

            return modelPayload;
        }

        public override string ToString()
        {
            return Name;
        }

        public ProcedureStep GetFirstStep()
        {
            if(procedureSteps.Count == 0)
            {
                return null;
            }
            return procedureSteps[0];
        }

    }
}
