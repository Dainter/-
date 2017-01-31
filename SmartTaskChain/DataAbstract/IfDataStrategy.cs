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
        void AcceptModification();
        //删

        //改

        //查

        //配置


    }
}
