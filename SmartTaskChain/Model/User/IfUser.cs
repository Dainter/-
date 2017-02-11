﻿using System;
using System.Collections.Generic;
using System.Xml;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    public interface IfUser
    {
        string Name { get; }
        string Type { get; }
        string Password { get; }
        List<UserGroup> UserGroups { get; }
        List<IfTask> SubmitTasks { get; }
        List<IfTask> HandleTasks { get; }

        void ExtractRelation(IfDataStrategy DataReader, MainDataSet dataset);
        void StoreRelation(IfDataStrategy DataReader, MainDataSet dataset);
        int GetTotalWork();
        XmlElement XMLSerialize();
    }
}
