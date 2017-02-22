using System;
using System.Collections.Generic;
using System.AddIn.Pipeline;
using System.AddIn.Contract;

namespace Contract
{
    [AddInContract]
    public interface KPICaculaterContract : IContract
    {
        double CaculateKPI(List<string> workload);
    }
}
