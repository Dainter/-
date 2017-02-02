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

        public Procedure BindingProcedure
        {
            get { return procedure; }
            set { procedure = value; }
        }

        public TaskType(string sName, string sDescription = "")
        {
            this.strName = sName;
            this.strDescription = sDescription;
        }

        public TaskType(XmlElement ModelPayload)
        {
            this.strName = Utility.GetText(ModelPayload, "Name");
            this.strDescription = Utility.GetText(ModelPayload, "Description");
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

    }
}
