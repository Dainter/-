using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    public class UserGroup
    {
        //Name
        string strName;
        //Description
        string strDescription;
        //Users[1:n]
        List<IfUser> users;
        //ProcedureStep[1:1]
        ProcedureStep procedureStep;

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
        public List<IfUser> Users
        {
            get { return users; }
        }
        public bool IsBindingStep
        {
            get
            {
                if (procedureStep == null)
                {
                    return false;
                }
                return true;
            }
        }
        public ProcedureStep BindingStep
        {
            get { return procedureStep; }
            set { procedureStep = value; }
        }

        public UserGroup(string sName, string sDescription = "")
        {
            this.strName = sName;
            this.strDescription = sDescription;
            this.users = new List<IfUser>();
            this.procedureStep = null;
        }

        public UserGroup(XmlElement ModelPayload)
        {
            this.strName = Utility.GetText(ModelPayload, "Name");
            this.strDescription = Utility.GetText(ModelPayload, "Description");
            this.users = new List<IfUser>();
            this.procedureStep = null;
        }

        public void UpdateRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            //Users[1:n]
            this.users.Clear();
            List<string> sUsers = DataReader.GetDNodesBySNodeandEdgeType(this.Name, this.Type, "Include");
            foreach (string username in sUsers)
            {
                this.users.Add(dataset.GetUserItem(username));
            }
            //ProcedureStep[1:1]
            Record record = DataReader.GetDNodeBySNodeandEdgeType(this.Name, this.Type, "InCharge");
            this.procedureStep = dataset.GetStepItem(record.Name);
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
    }
}
