using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskChain.DataAbstract
{
    public class Record
    {
        string strName;
        string strType;
        XmlElement payload;

        public string Name
        {
            get {return strName;}
        }
        public string Type
        {
            get { return strType; }
        }
        public XmlElement Payload
        { get { return payload; } }

        public Record(string sName, string sType, XmlElement pload)
        {
            strName = sName;
            strType = sType;
            payload = pload;
        }
    }
}
