using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskChainLib.Model
{
    public interface IfTask
    {
        string Name { get; }
        DateTime StartTime { get; }
        DateTime DeadLine { get; }
        string Memo { get; }

        //XmlNode XMLSerialize(XmlNode BusinessPayload);
    }
}
