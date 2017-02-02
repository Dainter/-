using System;
using System.Collections.Generic;
using System.Xml;
using SmartTaskChain.Model;

namespace SmartTaskChain.Business
{
    class Engineer : IfUser
    {
        User user;
        //employee number
        string strNumber;

        //User公共属性
        public string Name
        {
            get
            { return user.Name; }
        }
        public string Type
        {
            get { return this.GetType().Name; }
        }
        public string Password
        {
            get { return this.user.Password; }
        }
        public List<UserGroup> UserGroups
        {
            get { return this.user.UserGroups; }
        }
        public List<IfTask> SubmitTasks
        {
            get { return this.user.SubmitTasks; }
        }
        public List<IfTask> HandleTasks
        {
            get { return this.user.HandleTasks; }
        }
        //Engineer私有属性
        public string EmployeeNumber
        {
            get { return strNumber; }
        }

        public Engineer(string sName, string sPwd, string sNumber)
        {
            this.user = new User(sName, sPwd, Type);
            this.strNumber = sNumber;
        }

        public Engineer(XmlElement modelPayload)
        {
            this.user = new User(modelPayload);
            this.strNumber = Utility.GetText(Utility.GetNode(modelPayload, "BussinessPayload"), "EmployeeNumber");
        }

        public XmlElement XMLSerialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlText num_txt;
            XmlElement num_xml, businessPayload;

            businessPayload = doc.CreateElement("BussinessPayload");
            num_xml = doc.CreateElement("EmployeeNumber");


            num_txt = doc.CreateTextNode(this.EmployeeNumber);

            num_xml.AppendChild(num_txt);

            businessPayload.AppendChild(num_xml);

            return this.user.XMLSerialize(businessPayload);
        }
    }
}
