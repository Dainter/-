using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphDB.Core;

namespace SmartTaskChain.DataAbstract
{
    public class RelationShip
    {
        string strSourceName;
        string strSourceType;
        string strDestinationName;
        string strDestinationType;
        string strRelationType;
        string strRelationValue;

        public string SourceName
        {
            get { return strSourceName; }
        }
        public string SourceType
        {
            get { return strSourceType; }
        }
        public string DestinationName
        {
            get { return strDestinationName; }
        }
        public string DestinationType
        {
            get { return strDestinationType; }
        }
        public string RelationType
        {
            get { return strRelationType; }
        }
        public string RelationValue
        {
            get { return strRelationValue; }
        }

        public RelationShip(string sSName, string sSType,
                                        string sDName, string sDType,
                                        string sType, string sValue = "1")
        {
            strSourceName = sSName;
            strSourceType = sSType;
            strDestinationName = sDName;
            strDestinationType = sDType;
            strRelationType = sType;
            strRelationValue = sValue;
        }

        public RelationShip(Edge curEdge)
        {
            strSourceName = curEdge.Start.Name;
            strSourceType = curEdge.Start.Type;
            strDestinationName = curEdge.End.Name;
            strDestinationType = curEdge.End.Type;
            strRelationType = curEdge.Type;
            strRelationValue = curEdge.Value;
        }

    }
}
