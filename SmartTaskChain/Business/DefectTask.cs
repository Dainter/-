using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmartTaskChain.Model;

namespace SmartTaskChain.Business
{
    class DefectTask:IfTask
    {
        Task task;
        string strDefectDescription;

        public string Name
        {
            get
            {
                return task.Name;
            }
        }
        public string Type
        {
            get { return task.Type; }
        }
        public string BusinessType
        {
            get { return this.GetType().Name; }
        }
        public ProcedureStep CurrentStep
        {
            get { return this.task.CurrentStep; }
            set { this.task.CurrentStep = value; }
        }
        public IfUser Handler
        {
            get { return this.task.Handler; }
            set { this.task.Handler = value; }
        }
        public DateTime StartTime
        {
            get
            {
                return task.StartTime;
            }
        }
        public DateTime DeadLine
        {
            get
            {
                return task.DeadLine;
            }
        }
        public QLevel QLevel
        {
            get { return this.task.QLevel; }
            set { this.task.QLevel = value; }
        }
        public string Status
        {
            get { return this.task.Status; }
            set { this.task.Status = value; }
        }
        public string DelayReason
        {
            get { return this.task.DelayReason; }
            set { this.task.DelayReason = value; }
        }
        public string DefectDescription
        {
            get { return strDefectDescription; }
        }

        public DefectTask(string sName, DateTime dStart, DateTime dDead, string sDescription, bool IsImportant = false, bool IsEmergency = false)
        {
            this.task = new Task(sName,
                                        dStart, 
                                        dDead);
            //taskupdate
            this.strDefectDescription = sDescription;
        }

        public DefectTask(XmlElement modelPayload)
        {
            this.task = new Task(modelPayload);
            this.strDefectDescription = GetText(GetNode(modelPayload, "BussinessPayload"), "DefectDescription");
        }
        //工具函数，从xml节点中返回某个标签的标识的节点
        XmlElement GetNode(XmlElement curNode, string sLabel)
        {
            if (curNode == null)
            {
                return null;
            }
            //遍历子节点列表
            foreach (XmlElement xNode in curNode.ChildNodes)
            {
                if (xNode.Name == sLabel)
                {//查找和指定内容相同的标签，返回其Innner Text
                    return xNode;
                }
            }
            return null;
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
            XmlText description_txt;
            XmlElement BusinessPayload, Defect_xml;

            BusinessPayload = doc.CreateElement("BussinessPayload");
            Defect_xml = doc.CreateElement("DefectDescription");
            description_txt = doc.CreateTextNode(this.DefectDescription);
            Defect_xml.AppendChild(description_txt);
            BusinessPayload.AppendChild(Defect_xml);

            return task.XMLSerialize(BusinessPayload);
        }
    }

}
