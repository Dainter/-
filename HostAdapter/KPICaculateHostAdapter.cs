using System;
using System.Collections.Generic;
using System.AddIn.Pipeline;
using System.AddIn.Contract;

namespace HostAdapter
{
    [HostAdapter]
    public class KPICaculateHostAdapter : HostView.KPICaculaterHostView
    {
        private Contract.KPICaculaterContract contract;
        private ContractHandle contractHandle;

        public KPICaculateHostAdapter(Contract.KPICaculaterContract contract)
        {
            this.contract = contract;
            contractHandle = new ContractHandle(contract);
        }

        public override double CaculateKPI(List<string> workload)
        {
            return contract.CaculateKPI(workload);
        }
    }
}
