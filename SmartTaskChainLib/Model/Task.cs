using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphDB.Core;

namespace TaskChainLib.Model
{
    [Serializable]
    class Task:IfTask
    {
        string strName;
        DateTime datStartTime;
        DateTime datDeadLine;
        string strMemo;
        XmlNode BusinessPayload;
        

        public string Name
        {
            get { return strName; }
        }

        public DateTime StartTime
        {
            get { return DeadLine; }
        }

        public DateTime DeadLine
        {
            get { return DeadLine; }
        }

        public string Memo
        {
            get { return strMemo; }
        }

        //Name
        //TaskType[1:1]
        //Sumitter[1:1]
        //Handler[1:1]
        //CurrentStep[1:1]
        //Priority[1:1]
        //StartTime
        //DeadLine
        //CompleteTime
        //TaskStatus
        //DelayReason(if TaskStatus == Delay then not null)
        //Memo

        public Task()
        {
            strName = "NewTask";
            datStartTime = DateTime.Now;
            datDeadLine = DateTime.Now;
            strMemo = "Test Serialize Function";
        }

        public XmlNode XMLSerialize(XmlNode BusinessPayload)
        {
            return null;
        }

    }
}
