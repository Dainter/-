﻿using System;
using System.Xml;
using System.Collections.Generic;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    class Task
    {
        //Name
        string strName;
        //TaskType[1:1]
        TaskType taskType;
        //Sumitter[1:1]
        IfUser usrSubmitter;
        //Handler[1:1]
        IfUser usrHandler;
        //StartTime
        DateTime datStartTime;
        //DeadLine
        DateTime datDeadLine;
        //Priority[1:1]
        QLevel eQlevel;
        //TaskStatus
        TaskStatus.EnumTaskStatus taskStatus;
        double dubPriority;

        public string Name
        {
            get { return strName; }
        }
        public string Type
        {
            get { return this.GetType().Name; }
        }
        public TaskType BusinessType
        {
            get { return this.taskType; }
            set { this.taskType = value; }
        }
        public IfUser Submitter
        {
            get { return usrSubmitter; }
            set { usrSubmitter = value; }
        }
        public bool IsBindingProcedure
        {
            get { return this.taskType.IsUseProcedure; }
        }
        public IfUser Handler
        {
            get { return usrHandler; }
            set { usrHandler = value; }
        }
        public DateTime StartTime
        {
            get { return datStartTime; }
        }
        public DateTime DeadLine
        {
            get { return datDeadLine; }
        }
        public QLevel QLevel
        {
            get { return eQlevel; }
            set { eQlevel = value; }
        }
        public string Status
        {
            get { return TaskStatus.ToString(taskStatus); }
            set { taskStatus = TaskStatus.ToEnum(value); }
        }
        public double Priority
        {
            get { return dubPriority; }
            set { dubPriority = value; }
        }

        public Task(string sName, DateTime dStart, DateTime dDead)
        {
            this.strName = sName;
            this.taskType = null;
            this.usrSubmitter = null;
            this.usrHandler = null;
            this.datStartTime = dStart;
            this.datDeadLine = dDead;
            this.eQlevel = null;
            this.taskStatus = TaskStatus.EnumTaskStatus.Process;
            this.dubPriority = 0.0;
        }

        public Task(XmlElement ModelPayload)
        {
            this.strName = Utility.GetText(ModelPayload, "Name");
            this.taskType = null;
            this.usrSubmitter = null;
            this.usrHandler = null;
            this.datStartTime = Convert.ToDateTime(Utility.GetText(ModelPayload, "StartTime"));
            this.datDeadLine = Convert.ToDateTime(Utility.GetText(ModelPayload, "DeadLine"));
            this.eQlevel = null;
            this.taskStatus = TaskStatus.ToEnum(Utility.GetText(ModelPayload, "Status"));
            this.dubPriority = 0.0;
        }

        public void UpdatePriority()
        {
            if (BusinessType == null || QLevel == null)
            {
                return;
            }
            dubPriority = CalPriority(DeadLine - StartTime, DeadLine - DateTime.Now, BusinessType.Priority, QLevel.Priority);
        }

        double CalPriority(TimeSpan total, TimeSpan remain, int iCategory, int iQlevel)
        {
            if(remain.TotalHours == 0)
            {
                return (iCategory * 10 + iQlevel) * 1.0 * (total.TotalHours / 1);
            }
            return (iCategory * 10 + iQlevel) * 1.0 * (total.TotalHours / remain.TotalHours);
        }

        public XmlElement XMLSerialize(XmlElement BusinessPayload)
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, start_txt, dead_txt, status_txt;
            XmlElement name_xml, start_xml, dead_xml, status_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            start_xml = doc.CreateElement("StartTime");
            dead_xml = doc.CreateElement("DeadLine");
            status_xml = doc.CreateElement("Status");

            name_txt = doc.CreateTextNode(this.Name);
            start_txt = doc.CreateTextNode(this.StartTime.ToString());
            dead_txt = doc.CreateTextNode(this.DeadLine.ToString());
            status_txt = doc.CreateTextNode(this.Status);

            name_xml.AppendChild(name_txt);
            start_xml.AppendChild(start_txt);
            dead_xml.AppendChild(dead_txt);
            status_xml.AppendChild(status_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(start_xml);
            modelPayload.AppendChild(dead_xml); 
            modelPayload.AppendChild(status_xml);
            modelPayload.AppendChild(doc.ImportNode(BusinessPayload, true));

            return modelPayload;
        }

    }
}
