using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using SmartTaskChain.Model;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Business
{
    class Manager: IfUser
    {
        User user;
        //employee number
        string strNumber;
        //InferiorUsers[1:n]
        List<IfUser> uInferiors;

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
        //Manager私有属性
        public string EmployeeNumber
        {
            set { strNumber = value; }
            get { return strNumber; }
        }
        public List<IfUser> Inferiors
        {
            get { return this.uInferiors; }
        }

        public Manager(string sName, string sPwd, string sNumber)
        {
            this.user = new User(sName, sPwd, Type);
            this.strNumber = sNumber;
            this.uInferiors = new List<IfUser>();

        }

        public Manager(XmlElement modelPayload)
        {
            this.user = new User(modelPayload);
            this.uInferiors = new List<IfUser>();
            this.strNumber = Utility.GetText(Utility.GetNode(modelPayload, "BussinessPayload"), "EmployeeNumber");
        }

        public void ExtractRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            this.user.ExtractRelation(DataReader, dataset);
            //InferiorUsers[1:n]
            List<string> inferiors = DataReader.GetDNodesBySNodeandEdgeType(this.Name, this.Type, "Inferior");
            foreach (string username in inferiors)
            {
                this.uInferiors.Add(dataset.GetUserItem(username));
            }
        }

        public void StoreRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            this.user.StoreRelation(DataReader, dataset);
            RelationShip newRelation;
            if (this.uInferiors.Count > 0)
            {
                //InferiorUsers[1:n]
                foreach (IfUser curUser in this.uInferiors)
                {
                    newRelation = new RelationShip(this.Name, this.Type, curUser.Name, curUser.Type, "Inferior", "1");
                    DataReader.InsertRelationShip(newRelation);
                }
            }
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
