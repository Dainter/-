using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphDB;

namespace SmartTaskChain.DataAbstract
{
    public class DataStrategyFactory
    {
        private DataStrategyFactory()
        {
            //查表，维护类型列表
        }     //(1)

        private static DataStrategyFactory _factory; //(2)
        private static IfDataStrategy _DBreader;
        public static DataStrategyFactory GetFactory() //(3)
        {
            if (_factory == null)
                _factory = new DataStrategyFactory();
            return _factory;
        }

        public IfDataStrategy GetDataReader (string sPath)
        {
            ErrorCode err = ErrorCode.NoError;
            //查表
            if(_DBreader == null)
            {
                switch(GetExtension(sPath))
                {
                    case ".xml":
                        _DBreader = new GraphDBStrategy(sPath, ref err);
                        if(err != ErrorCode.NoError)
                        {
                            _DBreader = null;
                        }
                        break;
                    case ".mdb":
                        break;
                    case ".mdf":
                        break;
                    case ".xslx":
                        break;
                    default:
                        _DBreader = null;
                        break;
                }
            }
            return _DBreader;
        }

        private string GetExtension(string sPath)
        {
            int index = sPath.LastIndexOf(".");
            if(index <0)
            {
                return "";
            }
            return sPath.Substring(index);
        }
    }
}
