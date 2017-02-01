using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using SmartTaskChain.Model;

namespace SmartTaskChain.Business
{
    class Manager: IfUser
    {
        //InferiorUsers[1:n]
        List<IfUser> usrInferiors;
        public List<IfUser> Inferiors
        {
            get { return usrInferiors; }
        }

        public Manager()
        {

            this.usrInferiors = new List<IfUser>();

        }

        public Manager(XmlElement BusinessPayload)
        {

            this.usrInferiors = new List<IfUser>();
        }

    }
}
