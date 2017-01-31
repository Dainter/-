using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskChain.Model
{
    class TaskTypeFactory
    {
        private TaskTypeFactory()
        {
            //查表，维护类型列表
        }     //(1)

        private static TaskTypeFactory _factory; //(2)

        public static TaskTypeFactory GetFactory() //(3)
        {
            if (_factory == null)
                _factory = new TaskTypeFactory();
            return _factory;
        }

        public TaskType GetTaskType(string sTypeName)
        {
            //查表
            return new TaskType(sTypeName);
        }
    }
}
