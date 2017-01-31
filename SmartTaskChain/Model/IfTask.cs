using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskChain.Model
{
    public interface IfTask
    {
        string Name { get; }
        //TaskType Type { get; }
        DateTime StartTime { get; }
        DateTime DeadLine { get; }

        //XmlNode XMLSerialize(XmlNode BusinessPayload);
    }
}
