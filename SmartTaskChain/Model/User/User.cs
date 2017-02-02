using System;
using System.Collections.Generic;
using System.Xml;

namespace SmartTaskChain.Model
{

    public class User
    {
        //Name
        string strName;
        string strPassword;
        string strBusinessType;
        //UserRoles[1:n]
        List<UserGroup> usrGroups;
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
            get { return this.strBusinessType; }
        }
        public string Password
        {
            get { return strPassword; }
        }
        public List<UserGroup> UserGroups
        {
            get { return usrGroups; }
        }
        public List<IfTask> SubmitTasks
        {
            get { return taskSubmit; }
        }
        public List<IfTask> HandleTasks
        {
            get { return taskHandle; }
        }

        public User(string sName, string sPwd, string sBType)
        {
            this.strName = sName;
            this.strBusinessType = sBType;
            this.strPassword = sPwd;
            this.usrGroups = new List<UserGroup>();
            this.taskSubmit =  new List<IfTask>();
            this.taskHandle = new List<IfTask>();
        }

        public User(XmlElement ModelPayload)
        {
            this.strName = Utility.GetText(ModelPayload, "Name");
            this.strPassword = Utility.GetText(ModelPayload, "Password");
            this.usrGroups = new List<UserGroup>();
            this.taskSubmit = new List<IfTask>();
            this.taskHandle = new List<IfTask>();
        }

        public XmlElement XMLSerialize(XmlElement BusinessPayload)
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, pwd_txt;
            XmlElement name_xml, pwd_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            pwd_xml = doc.CreateElement("Password");

            name_txt = doc.CreateTextNode(this.Name);
            pwd_txt = doc.CreateTextNode(this.Password);

            name_xml.AppendChild(name_txt);
            pwd_xml.AppendChild(pwd_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(pwd_xml);
            modelPayload.AppendChild(doc.ImportNode(BusinessPayload, true));

            return modelPayload;
        }

    }
}
