using System;
using System.Collections.Generic;
using System.Xml;
using SmartTaskChain.DataAbstract;

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
            this.strBusinessType = Utility.GetText(ModelPayload, "Type");
            this.usrGroups = new List<UserGroup>();
            this.taskSubmit = new List<IfTask>();
            this.taskHandle = new List<IfTask>();
        }

        public void ExtractRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            //UserRoles[1:n]
            this.usrGroups.Clear();
            List <string> groups = DataReader.GetDNodesBySNodeandEdgeType(this.Name, this.Type, "BelongTo");
            foreach (string group in groups)
            {
                this.usrGroups.Add(dataset.GetGroupItem(group));
            }
            List<string> tasks;
            //SubmitTasks[1:n]
            this.taskSubmit.Clear();
            tasks = DataReader.GetDNodesBySNodeandEdgeType(this.Name, this.Type, "Submit");
            foreach (string taskname in tasks)
            {
                this.taskSubmit.Add(dataset.GetTaskItem(taskname));
            }
            //HandleTasks[1:n]
            this.taskHandle.Clear();
            tasks = DataReader.GetDNodesBySNodeandEdgeType(this.Name, this.Type, "Handle");
            foreach (string taskname in tasks)
            {
                this.taskHandle.Add(dataset.GetTaskItem(taskname));
            }
        }

        public void StoreRelation(IfDataStrategy DataReader, MainDataSet dataset)
        {
            RelationShip newRelation;
            if (this.usrGroups.Count > 0)
            {
                //UserRoles[1:n]
                foreach (UserGroup curGroup in this.usrGroups)
                {
                    newRelation = new RelationShip(this.Name, this.Type, curGroup.Name, curGroup.Type, "BelongTo", "1");
                    DataReader.InsertRelationShip(newRelation);
                }
            }
            if (this.taskSubmit.Count > 0)
            {
                //SubmitTasks[1:n]
                foreach (IfTask curTask in this.taskSubmit)
                {
                    newRelation = new RelationShip(this.Name, this.Type, curTask.Name, curTask.Type, "Submit", "1");
                    DataReader.InsertRelationShip(newRelation);
                }
            }
            if (this.taskHandle.Count > 0)
            {
                //HandleTasks[1:n]
                foreach (IfTask curTask in this.taskHandle)
                {
                    newRelation = new RelationShip(this.Name, this.Type, curTask.Name, curTask.Type, "Handle", "1");
                    DataReader.InsertRelationShip(newRelation);
                }
            }
        }

        public XmlElement XMLSerialize(XmlElement BusinessPayload)
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, type_txt, pwd_txt;
            XmlElement name_xml, type_xml, pwd_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            type_xml = doc.CreateElement("Type");
            pwd_xml = doc.CreateElement("Password");

            name_txt = doc.CreateTextNode(this.Name);
            type_txt = doc.CreateTextNode(this.Type);
            pwd_txt = doc.CreateTextNode(this.Password);

            name_xml.AppendChild(name_txt);
            type_xml.AppendChild(type_txt);
            pwd_xml.AppendChild(pwd_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(type_xml);
            modelPayload.AppendChild(pwd_xml);
            modelPayload.AppendChild(doc.ImportNode(BusinessPayload, true));

            return modelPayload;
        }

        public int GetTotalWork()
        {
            int intTotal = 0;

            foreach(IfTask curTask in taskHandle)
            {
                intTotal += curTask.BusinessType.Priority * 10 + curTask.QLevel.Priority;
            }
            return intTotal;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
