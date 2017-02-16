using System;
using System.Collections.Generic;
using System.Xml;
using SmartTaskChain.Model;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Business
{
    class Customer : IfUser
    {
        User user;
        string strPhone;
        string strCompany;

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
            set { this.user.Password = value; }
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
        //Customer私有属性
        public string Phone
        {
            get { return strPhone; }
        }
        public string Company
        {
            get { return strCompany; }
        }

        public Customer(string sName, string sPwd, string sPhone, string sCompany)
        {
            this.user = new User(sName, sPwd, Type);
            this.strPhone = sPhone;
            this.strCompany = sCompany;
        }

        public Customer(XmlElement modelPayload)
        {
            this.user = new User(modelPayload);
            this.strPhone = Utility.GetText(Utility.GetNode(modelPayload, "BussinessPayload"), "Phone");
            this.strCompany = Utility.GetText(Utility.GetNode(modelPayload, "BussinessPayload"), "Company");

        }

        public void ExtractRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            this.user.ExtractRelation(DataReader, dataset);
        }

        public void StoreRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            this.user.StoreRelation(DataReader, dataset);
        }

        public XmlElement XMLSerialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlText phone_txt, company_txt;
            XmlElement phone_xml, company_xml, businessPayload;

            businessPayload = doc.CreateElement("BussinessPayload");
            phone_xml = doc.CreateElement("Phone");
            company_xml = doc.CreateElement("Company");

            phone_txt = doc.CreateTextNode(this.Phone);
            company_txt = doc.CreateTextNode(this.Company);

            phone_xml.AppendChild(phone_txt);
            company_xml.AppendChild(company_txt);

            businessPayload.AppendChild(phone_xml);
            businessPayload.AppendChild(company_xml);

            return this.user.XMLSerialize(businessPayload);
        }

        public int GetTotalWork()
        {
            return this.user.GetTotalWork();
        }

        public override string ToString()
        {
            return this.user.ToString();
        }
    }
}
