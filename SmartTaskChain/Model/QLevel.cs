using System;
using System.Collections.Generic;
using System.Xml;

namespace SmartTaskChain.Model
{
    public class QLevel
    {
        string strName;
        int intPriority;
        string strDescription;

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
        public int Priority
        {
            get { return intPriority; }
        }

        public QLevel(bool IsImportant = false, bool IsEmergency = false)
        {
            if (IsImportant == true)
            {
                if (IsEmergency == true)
                {
                    this.strName = "Q1";
                    this.strDescription = "紧急且重要";
                    this.intPriority = 40;
                }
                else
                {
                    this.strName = "Q2";
                    this.strDescription = "重要不紧急";
                    this.intPriority = 30;
                }
            }
            else
            {
                if (IsEmergency == true)
                {
                    this.strName = "Q3";
                    this.strDescription = "重要不紧急";
                    this.intPriority = 20;
                }
                else
                {
                    this.strName = "Q4";
                    this.strDescription = "不重要不紧急";
                    this.intPriority = 10;
                }
            }
        }

        public QLevel(string sDescription)
        {
            strName = sDescription;

            switch(sDescription)
            {
                case "Q1":
                    this.strDescription = "紧急且重要";
                    this.intPriority = 40;
                    break;
                case "Q2":
                    this.strDescription = "重要不紧急";
                    this.intPriority = 30;
                    break;
                case "Q3":
                    this.strDescription = "重要不紧急";
                    this.intPriority = 20;
                    break;
                case "Q4":
                    this.strDescription = "不重要不紧急";
                    this.intPriority = 10;
                    break;
                default:
                    break;
            }
        }

        public QLevel(XmlElement ModelPayload)
        {
            this.strName = Utility.GetText(ModelPayload, "Name");
            this.strDescription = Utility.GetText(ModelPayload, "Description");
            this.intPriority = Convert.ToInt32(Utility.GetText(ModelPayload, "Priority"));
        }

        public XmlElement XMLSerialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, desc_txt, priority_txt;
            XmlElement name_xml, desc_xml, priority_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            desc_xml = doc.CreateElement("Description");
            priority_xml = doc.CreateElement("Priority");

            name_txt = doc.CreateTextNode(this.Name);
            desc_txt = doc.CreateTextNode(this.Description);
            priority_txt = doc.CreateTextNode(this.intPriority.ToString());

            name_xml.AppendChild(name_txt);
            desc_xml.AppendChild(desc_txt);
            priority_xml.AppendChild(priority_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(desc_xml);
            modelPayload.AppendChild(priority_xml);

            return modelPayload;
        }

    }
}
