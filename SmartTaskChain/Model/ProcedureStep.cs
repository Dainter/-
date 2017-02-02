using System;
using System.Collections.Generic;
using System.Xml;

namespace SmartTaskChain.Model
{
    public class ProcedureStep
    {
        //Name
        string strName;
        //Index
        int intIndex;
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

        public ProcedureStep(string sName, int iIndex, string sDescription = "")
        {
            this.strName = sName;
            this.intIndex = iIndex;
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
            this.strDescription = Utility.GetText(ModelPayload, "Description");
            this.procedure = null;
            this.nextStep = null;
            this.preStep = null;
            this.handleRole = null;
        }

        public XmlElement XMLSerialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, index_txt, desc_txt;
            XmlElement name_xml, index_xml, desc_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            index_xml = doc.CreateElement("Index");
            desc_xml = doc.CreateElement("Description");

            name_txt = doc.CreateTextNode(this.Name);
            index_txt = doc.CreateTextNode(this.Index.ToString());
            desc_txt = doc.CreateTextNode(this.Description);

            name_xml.AppendChild(name_txt);
            index_xml.AppendChild(index_txt);
            desc_xml.AppendChild(desc_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(index_xml);
            modelPayload.AppendChild(desc_xml);

            return modelPayload;
        }

    }
}
