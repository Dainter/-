using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskChain.Model
{
    class ModelFactory
    {
        private ModelFactory()
        {
            //查表，维护Qlevel列表
        }     //(1)

        private static ModelFactory _factory; //(2)

        public static ModelFactory GetFactory() //(3)
        {
            if (_factory == null)
                _factory = new ModelFactory();
            return _factory;
        }

        public QLevel GetQlevel(bool IsEmergency = false, bool IsImportant = false)
        {
            //查表
            return new QLevel(IsEmergency, IsImportant);
        }

        public QLevel GetQlevel(string sDescription)
        {
            //查表，找不到报错
            return new QLevel(sDescription);
        }

        public TaskType GetTaskType(string sName)
        {
            //查表，找不到可以创建新类型
            return new TaskType(sName);
        }

        public Procedure GetProcedure(string sName)
        {
            //查表，找不到报错
            return new Procedure(sName);
        }
    }
}
