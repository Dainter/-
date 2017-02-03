using System;
using System.Collections.Generic;
using System.Xml;
using GraphDB.Core;
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

        public Record(Node curNode)
        {
            strName = curNode.Name;
            strType = curNode.Type;
            payload = curNode.Payload;
        }

    }
}
