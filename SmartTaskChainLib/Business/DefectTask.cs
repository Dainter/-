using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TaskChainLib.Model;

namespace TaskChainLib.Business
{
    public class DefectTask:IfTask
    {
        Task task;

        public DefectTask()
        {
            task = new Task();
        }

        public DateTime DeadLine
        {
            get
            {
                return task.DeadLine;
            }
        }

        public string Memo
        {
            get
            {
                return task.Memo;
            }
        }

        public string Name
        {
            get
            {
                return task.Name;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return task.StartTime;
            }
        }

        public XmlNode XMLSerialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode BusinessPayload = doc.CreateNode(XmlNodeType.Element, "DefectTask", "BussinessPayload");
            return task.XMLSerialize(BusinessPayload);
        }
    }
}
