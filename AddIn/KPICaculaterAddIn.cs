using System;
using System.Collections.Generic;
using System.AddIn;

namespace KPICaculaterAddIn
{
    [AddIn("KPI Caculater", Version = "1.0.0.0", Publisher = "Preussen",
            Description = "Caculate the KPI for each staff.")]
    public class KPICaculaterAddIn : AddInView.KPICaculaterAddInView
    {
        public override double CaculateKPI(List<string> workload)
        {
            double dubKpi = 0.0;
            if(workload == null)
            {
                return dubKpi;
            }
            foreach(string curTask in workload)
            {
                switch(curTask)
                {
                    case "Q1":
                        dubKpi += 40;
                        break;
                    case "Q2":
                        dubKpi += 30;
                        break;
                    case "Q3":
                        dubKpi += 20;
                        break;
                    case "Q4":
                        dubKpi += 10;
                        break;
                }
            }
            return dubKpi;
        }
    }
}
