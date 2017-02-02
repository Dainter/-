using System;
using System.Xml;
using System.Collections.Generic;

namespace SmartTaskChain.Model
{
    public interface IfTask
    {
        string Name { get; }
        string Type { get; }
        TaskType BusinessType { get; set; }
        bool IsBindingProcedure { get; }
        IfUser Submitter { get; set; }
        IfUser Handler { get; set; }
        DateTime StartTime { get; }
        DateTime DeadLine { get; }
        QLevel QLevel { get; set; }
        string Status { get; set; }
        string DelayReason { get; set; }

        //XmlElement XMLSerialize(XmlElement BusinessPayload);
    }
}
