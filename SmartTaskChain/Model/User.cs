using System;
using System.Collections.Generic;
using System.Xml;

namespace SmartTaskChain.Model
{

    public class User
    {
        //Name
        string strName;
        //UserRoles[1:n]
        List<UserRole> usrRoles;
        //SubmitTasks[1:n]
        List<IfTask> taskSubmit;
        //HandleTasks[1:n]
        List<IfTask> taskHandle;

        public string Name
        {
            get
            { return strName; }
        }
        public string Type
        {
            get { return this.GetType().Name; }
        }
        public List<UserRole> UserRoles
        {
            get { return usrRoles; }
        }

        public List<IfTask> SubmitTasks
        {
            get { return taskSubmit; }
        }
        public List<IfTask> HandleTasks
        {
            get { return taskHandle; }
        }

        public User(string sName)
        {
            this.strName = sName;
            this.usrRoles = new List<UserRole>();
            this.taskSubmit =  new List<IfTask>();
            this.taskHandle = new List<IfTask>();
        }

        public User(XmlElement ModelPayload)
        {
            this.strName = GetText(ModelPayload, "Name");
            this.usrRoles = new List<UserRole>();
            this.taskSubmit = new List<IfTask>();
            this.taskHandle = new List<IfTask>();
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
            XmlText name_txt;
            XmlElement name_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");

            name_txt = doc.CreateTextNode(this.Name);

            name_xml.AppendChild(name_txt);

            modelPayload.AppendChild(name_xml);

            return modelPayload;
        }

    }
}
