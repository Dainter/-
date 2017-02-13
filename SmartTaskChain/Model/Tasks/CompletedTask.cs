using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;

namespace SmartTaskChain.Model
{
    public class CompletedTask
    {
        //Name
        string strName;
        //TaskType[1:1]
        string strType;
        //Sumitter[1:1]
        string strSubmitter;
        //Handler
        string strHandler;
        //StartTime
        DateTime datStartTime;
        //DeadLine
        DateTime datDeadLine;
        //CompleteTime
        DateTime datCompletedTime;
        string strQlevel;
        string strDescription;

        public string Name
        {
            get
            {
                return this.strName;
            }
        }
        public string Type
        {
            get { return this.strType; }
        }
        public string Submitter
        {
            get { return this.strSubmitter; }
        }
        public string Handler
        {
            get { return this.strHandler; }
        }
        public DateTime StartTime
        {
            get
            {
                return this.datStartTime;
            }
        }
        public DateTime DeadLine
        {
            get
            {
                return this.datDeadLine;
            }
        }
        public DateTime CompletedTime
        {
            get { return this.datCompletedTime; }
        }
        public bool IsDelay
        {
            get
            {
                if (this.datCompletedTime.CompareTo(this.datDeadLine) == 1)
                {
                    return true;
                }
                return false;
            }
        }
        public string QLevel
        {
            get { return this.strQlevel; }
        }
        public string Description
        {
            get { return strDescription; }
        }

        public CompletedTask(IfTask curTask)
        {
            this.strName = curTask.Name;
            this.strType = curTask.BusinessType.Name;
            this.strSubmitter = curTask.Submitter.Name;
            this.strHandler = curTask.Handler.Name;
            if (curTask.IsBindingProcedure == true)
            {
                this.strHandler = ExtractHandlerName(curTask.Description);
            }
            this.datStartTime = curTask.StartTime;
            this.datDeadLine = curTask.DeadLine;
            this.datCompletedTime = DateTime.Now;
            this.strQlevel = curTask.QLevel.Name;
            this.strDescription = curTask.Description;
        }

        private string ExtractHandlerName(string sDesc)
        {
            const string strExtractPattern = @"Step:[\u4E00-\u9FA5A-Za-z0-9_ ]*Handler:[\u4E00-\u9FA5A-Za-z0-9_ ]*";  //匹配目标"Step:+Handler:"组合
            string strName = "";
            MatchCollection matches;
            Regex regObj;

            regObj = new Regex(strExtractPattern);//正则表达式初始化，载入匹配模式
            matches = regObj.Matches(sDesc);//正则表达式对分词结果进行匹配
            foreach(Match match in matches)
            {
                strName += match.ToString() + "\n";
            }
            return strName;
        }

        public CompletedTask(XmlElement Payload)
        {
            this.strName = Utility.GetText(Payload, "Name");
            this.strType = Utility.GetText(Payload, "Type");
            this.strSubmitter = Utility.GetText(Payload, "Submitter");
            this.strHandler = Utility.GetText(Payload, "Handler");
            this.datStartTime = Convert.ToDateTime(Utility.GetText(Payload, "StartTime"));
            this.datDeadLine = Convert.ToDateTime(Utility.GetText(Payload, "DeadLine"));
            this.datCompletedTime = Convert.ToDateTime(Utility.GetText(Payload, "CompletedTime"));
            this.strQlevel = Utility.GetText(Payload, "QLevel");
            this.strDescription = Utility.GetText(Payload, "Description");
        }

        public XmlElement XMLSerialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlText name_txt, type_txt, sub_txt,hand_txt, start_txt, dead_txt, comp_txt, qlevel_txt, desc_txt;
            XmlElement name_xml, type_xml, sub_xml, hand_xml,start_xml, dead_xml, comp_xml, qlevel_xml, desc_xml, modelPayload;

            modelPayload = doc.CreateElement("Payload");
            name_xml = doc.CreateElement("Name");
            type_xml = doc.CreateElement("Type");
            sub_xml = doc.CreateElement("Submitter");
            hand_xml = doc.CreateElement("Handler");
            start_xml = doc.CreateElement("StartTime");
            dead_xml = doc.CreateElement("DeadLine");
            comp_xml = doc.CreateElement("CompletedTime");
            qlevel_xml = doc.CreateElement("QLevel");
            desc_xml = doc.CreateElement("Description");

            name_txt = doc.CreateTextNode(this.Name);
            type_txt = doc.CreateTextNode(this.Type);
            sub_txt = doc.CreateTextNode(this.Submitter);
            hand_txt = doc.CreateTextNode(this.Handler);
            start_txt = doc.CreateTextNode(this.StartTime.ToString());
            dead_txt = doc.CreateTextNode(this.DeadLine.ToString());
            comp_txt = doc.CreateTextNode(this.CompletedTime.ToString());
            qlevel_txt = doc.CreateTextNode(this.QLevel);
            desc_txt = doc.CreateTextNode(this.Description);

            name_xml.AppendChild(name_txt);
            type_xml.AppendChild(type_txt);
            sub_xml.AppendChild(sub_txt);
            hand_xml.AppendChild(hand_txt);
            start_xml.AppendChild(start_txt);
            dead_xml.AppendChild(dead_txt);
            comp_xml.AppendChild(comp_txt);
            qlevel_xml.AppendChild(qlevel_txt);
            desc_xml.AppendChild(desc_txt);

            modelPayload.AppendChild(name_xml);
            modelPayload.AppendChild(type_xml);
            modelPayload.AppendChild(sub_xml);
            modelPayload.AppendChild(hand_xml);
            modelPayload.AppendChild(start_xml);
            modelPayload.AppendChild(dead_xml);
            modelPayload.AppendChild(comp_xml);
            modelPayload.AppendChild(qlevel_xml);
            modelPayload.AppendChild(desc_xml);

            return modelPayload;
        }

    }
}
