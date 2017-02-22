using System;
using System.Collections.Generic;
using System.AddIn.Pipeline;

namespace AddInAdapter
{
    [AddInAdapter]
    public class KPICaculaterAdapter : ContractBase, Contract.KPICaculaterContract
    {
        private AddInView.KPICaculaterAddInView view;

        public KPICaculaterAdapter(AddInView.KPICaculaterAddInView view)
        {
            this.view = view;
        }

        public double CaculateKPI(List<string> workload)
        {
            return view.CaculateKPI(workload);
        }
    }
}
