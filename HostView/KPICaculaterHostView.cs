using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HostView
{
    public abstract class KPICaculaterHostView
    {
        public abstract double CaculateKPI(List<string> workload);
    }
}
