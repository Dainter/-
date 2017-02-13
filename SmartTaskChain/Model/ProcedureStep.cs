using System;
using System.Collections.Generic;
using System.Xml;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    public class ProcedureStep: IComparable
    {
        //Name
        string strName;
        //Index
        int intIndex;
        //IsFeedBack
        bool bolIsFeedback;
        //Description
        string strDescription;
        //Procedure[1:1]
        Procedure procedure;
        //NextStep[1:1]
        ProcedureStep nextStep;
        //PreviousStep[1:1]
        ProcedureStep preStep;
        //HandleRole[1:1]
        UserGroup handleRole;


        public string Name
        {
            get
            { return strName; }
        }
        public string Type
        {
            get { return this.GetType().Name; }
        }
        public int Index
        {
            get { return intIndex; }
        }
        public bool IsFeedback
        {
            get { return bolIsFeedback; }
        }
        public string Description
        {
            get
            { return strDescription; }
        }
        public Procedure BelongToProcedure
        {
            get { return procedure; }
            set { procedure = value; }
        }
        public ProcedureStep NextStep
        {
            get { return nextStep; }
            set { nextStep = value; }
        }
        public ProcedureStep PreviousStep
        {
            get { return preStep; }
            set { preStep = value; }
        }
        public UserGroup HandleRole
        {
            get { return handleRole; }
            set { handleRole = value; }
        }

        public ProcedureStep(string sName, int iIndex, bool bIsFeedback, string sDescription = "")
        {
            this.strName = sName;
            this.intIndex = iIndex;
            this.bolIsFeedback = bIsFeedback;
            this.strDescription = sDescription;
            this.procedure = null;
            this.nextStep = null;
            this.preStep = null;
            this.handleRole = null;
        }

        public ProcedureStep(XmlElement ModelPayload)
        {
            this.strName = Utility.GetText(ModelPayload, "Name");
            this.intIndex = Convert.ToInt32(Utility.GetText(ModelPayload, "Index"));
            this.bolIsFeedback = Convert.ToBoolean(Utility.GetText(ModelPayload, "IsFeedback"));
            this.strDescription = Utility.GetText(ModelPayload, "Description");
            this.procedure = null;
            this.nextStep = null;
            this.preStep = null;
            this.handleRole = null;
        }

        public void ExtractRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            Record record;
            //Procedure
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "BelongTo");
            this.procedure = dataset.GetProcedureItem(record.Name);
            //NextStep
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "Next");
            this.nextStep = dataset.GetStepItem(record.Name);
            //PreviousStep
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "Previous");
            this.preStep = dataset.GetStepItem(record.Name);
            if(this.IsFeedback == true)
            {
                return;
            }
            //HandleRole
            record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "HandleBy");
            this.handleRole = dataset.GetGroupItem(record.Name);
        }

        public void StoreRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            RelationShip newRelation;
            if (this.procedure != null)
            {
                //Procedure
                newRelation = new RelationShip(this.Name, this.Type, this.procedure.Name, this.procedure.Type, "BelongTo", "1");
                DataReader.InsertRelationShip(newRelation);
            }
            if (this.nextStep != null)
            {
                //NextStep
                newRelation = new RelationShip(this.Name, this.Type, this.nextStep.Name, this.nextStep.Type, "Next", "1");
                DataReader.InsertRelationShip(newRelation);
            }
            if (this.preStep != null)
            {
                //PreviousStep
                newRelation = new RelationShip(this.Name, this.Type, this.preStep.Name, this.preStep.Type, "Previous", "1");
                DataReader.InsertRelationShip(newRelation);
            }
            if (this.IsFeedback == true)
            {
                return;
            }
            if (this.handleRole != null)
            {
                //HandleRole
                newRelation = new RelationShip(this.Name, this.Type, this.handleRole.Name, this.handleRole.Type, "HandleBy", "1");
                DataReader.InsertRelationShip(newRelation);
            }
        }

        public XmlElement XMLSerialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, index_txt, feed_txt, desc_txt;
            XmlElement name_xml, index_xml, feed_xml, desc_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            index_xml = doc.CreateElement("Index");
            feed_xml = doc.CreateElement("IsFeedback");
            desc_xml = doc.CreateElement("Description");

            name_txt = doc.CreateTextNode(this.Name);
            index_txt = doc.CreateTextNode(this.Index.ToString());
            feed_txt = doc.CreateTextNode(this.IsFeedback.ToString());
            desc_txt = doc.CreateTextNode(this.Description);

            name_xml.AppendChild(name_txt);
            index_xml.AppendChild(index_txt);
            feed_xml.AppendChild(feed_txt);
            desc_xml.AppendChild(desc_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(index_xml);
            modelPayload.AppendChild(feed_xml);
            modelPayload.AppendChild(desc_xml);

            return modelPayload;
        }

        public int CompareTo(object other)
        {
            return this.Index.CompareTo(((ProcedureStep)other).Index);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
