using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskChain.Model
{
    class QLevelFactory
    {
        private QLevelFactory()
        {
            //查表，维护Qlevel列表
        }     //(1)

        private static QLevelFactory _factory; //(2)

        public static QLevelFactory GetFactory() //(3)
        {
            if (_factory == null)
                _factory = new QLevelFactory();
            return _factory;
        }

        public QLevel GetQlevel(bool IsEmergency = false, bool IsImportant = false)
        {
            //查表
            return new QLevel(IsEmergency, IsImportant);
        }

        public QLevel GetQlevel(string sDescription)
        {
            //查表
            return new QLevel(sDescription);
        }
    }
}
