using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskChain.Model
{
    class TaskType
    {
        //Name = strBussinessType
        string strName;
        //Type
        string strType;
        //BussinessType
        //string strBussinessType;
        //Procedure(if IsDirectly = true then null)[1:1]
        bool bolIsUseProcedure;
        Procedure procedure;
        

        public string Name
        {
            get{ return strName; }
        }

        public string Type
        {
            get { return strType; }
        }

        public string BussinessType
        {
            get { return strName; }
        }

        public bool IsUseProcedure
        {
            get { return bolIsUseProcedure; }
            set { IsUseProcedure = value; }
        }

        public Procedure BindingProcedure
        {
            get { return procedure; }
            set { procedure = value; }
        }

        public TaskType(string sName, string sDescription = "", bool bIsUseProcedure = false, Procedure proce = null)
        {
            strName = sName;
            strType = "TaskType";
            bolIsUseProcedure = bIsUseProcedure;
            //挂接处理流程
            procedure = proce;
        }

    }
}
