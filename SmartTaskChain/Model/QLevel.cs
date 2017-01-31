using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

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
                    strName = "Q1";
                    strDescription = "紧急且重要";
                    intPriority = 40;
                }
                else
                {
                    strName = "Q2";
                    strDescription = "重要不紧急";
                    intPriority = 30;
                }
            }
            else
            {
                if (IsEmergency == true)
                {
                    strName = "Q3";
                    strDescription = "重要不紧急";
                    intPriority = 20;
                }
                else
                {
                    strName = "Q4";
                    strDescription = "不重要不紧急";
                    intPriority = 10;
                }
            }
        }

        public QLevel(string sDescription)
        {
            strName = sDescription;

            switch(sDescription)
            {
                case "Q1":
                    strDescription = "紧急且重要";
                    intPriority = 40;
                    break;
                case "Q2":
                    strDescription = "重要不紧急";
                    intPriority = 30;
                    break;
                case "Q3":
                    strDescription = "重要不紧急";
                    intPriority = 20;
                    break;
                case "Q4":
                    strDescription = "不重要不紧急";
                    intPriority = 10;
                    break;
                default:
                    break;
            }
        }

        public QLevel(XmlElement ModelPayload)
        {
            this.strName = GetText(ModelPayload, "Name");
            this.strDescription = GetText(ModelPayload, "Description");
            this.intPriority = Convert.ToInt32(GetText(ModelPayload, "Priority"));
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
