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
            Created = 1,
            Delay = 2,
            Abort = 3,
            Completed = 4,
        }

        public static string ToString(EnumTaskStatus eStatus)
        {
            switch(eStatus)
            {
                case EnumTaskStatus.Created:
                    return "Created";
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
                    return EnumTaskStatus.Created;
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
