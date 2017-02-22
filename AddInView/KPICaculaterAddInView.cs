using System;
using System.Collections.Generic;
using System.AddIn.Pipeline;

namespace AddInView
{
    [AddInBase]
    public abstract class KPICaculaterAddInView
    {
        public abstract double CaculateKPI(List<string> workload);
    }
}
