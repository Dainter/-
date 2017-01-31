using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GraphDB;

namespace SmartTaskChain.DataAbstract
{
    class GraphDBStrategy:IfDataStrategy
    {
        GraphDataBase gdb;

        public GraphDBStrategy(string sPath, ref ErrorCode err)
        {
            gdb = new GraphDataBase();
            gdb.OpenDataBase(sPath, ref err);
        }

        public int InsertRecord(Record newRecord)
        {
            ErrorCode err = ErrorCode.NoError;
            if (gdb == null)
            {
                return 0;
            }
            gdb.AddNodeData(newRecord.Name, newRecord.Type, newRecord.Payload, ref err);
            if (err != ErrorCode.NoError)
            {
                return 0;
            }
            return 1;
        }

        public int InsertRecords(List<Record> newRecords)
        {
            int intCount = 0;
            ErrorCode err = ErrorCode.NoError;
            if (gdb == null)
            {
                return 0;
            }
            foreach(Record newRecord in newRecords)
            {
                gdb.AddNodeData(newRecord.Name, newRecord.Type, newRecord.Payload, ref err);
                if (err == ErrorCode.NoError)
                {
                    intCount++;
                }
            }
            return intCount;
        }

        public int InsertRelationShip(RelationShip newRelation)
        {
            ErrorCode err = ErrorCode.NoError;
            if (gdb == null)
            {
                return 0;
            }
            gdb.AddEdgeData(newRelation.SourceName,
                                        newRelation.SourceType,
                                        newRelation.DestinationName,
                                        newRelation.DestinationType,
                                        newRelation.RelationType,
                                        ref err,
                                        newRelation.RelationValue);
            if (err != ErrorCode.NoError)
            {
                return 0;
            }
            return 1;
        }

        public int InsertRelationShips(List<RelationShip> newRelations)
        {
            int intCount = 0;
            ErrorCode err = ErrorCode.NoError;
            if (gdb == null)
            {
                return 0;
            }
            foreach (RelationShip newRelation in newRelations)
            {
                gdb.AddEdgeData(newRelation.SourceName,
                                        newRelation.SourceType,
                                        newRelation.DestinationName,
                                        newRelation.DestinationType,
                                        newRelation.RelationType,
                                        ref err,
                                        newRelation.RelationValue);
                if (err == ErrorCode.NoError)
                {
                    intCount++;
                }
            }
            return intCount;
        }

        public void AcceptModification()
        {
            ErrorCode err = ErrorCode.NoError;
            gdb.DataExport(ref err);
            if (err != ErrorCode.NoError)
            {
                return;
            }
        }
    }
}
