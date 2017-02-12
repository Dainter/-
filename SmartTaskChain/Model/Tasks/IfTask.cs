using System;
using System.Xml;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    public interface IfTask : IComparable<IfTask>
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
        string Description { get; set; }
        double Priority { get; set; }

        void ExtractRelation(IfDataStrategy DataReader, MainDataSet dataset);
        void StoreRelation(IfDataStrategy DataReader, MainDataSet dataset);

        void UpdatePriority();
        XmlElement XMLSerialize(XmlElement BusinessPayload = null);
    }
}
