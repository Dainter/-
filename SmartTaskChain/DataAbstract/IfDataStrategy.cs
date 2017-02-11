using System;
using System.Xml;
using System.Collections.Generic;

namespace SmartTaskChain.DataAbstract
{
    public interface IfDataStrategy
    {
        //增
        int InsertRecord(Record newRecord);
        int InsertRecords(List<Record> newRecords);
        int InsertRelationShip(RelationShip newRelation);
        int InsertRelationShips(List<RelationShip> newRelations);
        void ClearAll();
        //删

        //改

        //查
        List<Record> GetRecordList(string sType = null);
        List<RelationShip> GetRelationList(string sSType, string sDType, string strRType);
        Record GetDNodeBySNodeandEdgeType(string sSName, string sSType, string sRType);
        List<string> GetDNodesBySNodeandEdgeType(string sSName, string sSType, string sRType);
        //配置
        void AcceptModification();

    }
}
