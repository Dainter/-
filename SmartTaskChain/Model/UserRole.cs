using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskChain.Model
{
    public class UserRole
    {
        //Name
        string strName;
        //Description
        string strDescription;
        //Handler[1:n]
        List<IfUser> usrHandlers;
        //ProcedureStep[1:1]
        ProcedureStep procedureSteps;

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
        public List<IfUser> Handlers
        {
            get { return usrHandlers; }
        }
        public ProcedureStep BindingStep
        {
            get { return procedureSteps; }
            set { procedureSteps = value; }
        }


        public UserRole(string sName, string sDescription = "")
        {
            this.strName = sName;
            this.strDescription = sDescription;
            this.usrHandlers = new List<IfUser>();
            this.procedureSteps = null;
        }

        public UserRole(XmlElement ModelPayload)
        {
            this.strName = GetText(ModelPayload, "Name");
            this.strDescription = GetText(ModelPayload, "Description");
            this.usrHandlers = new List<IfUser>();
            this.procedureSteps = null;
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
