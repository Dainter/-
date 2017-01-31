using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskChain.Model
{
    class TaskStatus
    {
        public enum EnumTaskStatus
        {
            Unknown =0,
            Process = 1,
            Wait = 2,
            Rollback = 3,
            Delay = 4,
            Abort = 5,
            Completed = 6,
        }

        public static string ToString(EnumTaskStatus eStatus)
        {
            switch(eStatus)
            {
                case EnumTaskStatus.Process:
                    return "Created";
                case EnumTaskStatus.Wait:
                    return "Wait";
                case EnumTaskStatus.Rollback:
                    return "Rollback";
                case EnumTaskStatus.Delay:
                    return "Delay";
                case EnumTaskStatus.Abort:
                    return "Abort";
                case EnumTaskStatus.Completed:
                    return "Completed";
                default:
                    return "Unknown";
            }
        }

        public static EnumTaskStatus ToEnum(string strStatus)
        {
            switch (strStatus)
            {
                case "Created":
                    return EnumTaskStatus.Process;
                case "Wait":
                    return EnumTaskStatus.Wait;
                case "Rollback":
                    return EnumTaskStatus.Rollback;
                case "Delay":
                    return EnumTaskStatus.Delay;
                case "Abort":
                    return EnumTaskStatus.Abort;
                case "Completed":
                    return EnumTaskStatus.Completed;
                default:
                    return EnumTaskStatus.Unknown;
            }
        }

    }
}
