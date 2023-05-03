using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models
{
    public class BaseHealthModel
    {
        public BaseHealthModel(long memoryTotal, double memoryUsed, long hddTotal, double hddUsed, double cpuUsed)
        {
            MemoryTotal = memoryTotal;
            MemoryUsed = (long)memoryUsed;

            if (MemoryTotal > 0 && MemoryUsed > 0)
                MemoryUsedPercent = (int)(((double)memoryUsed / memoryTotal) * 100);

            HDDTotal = hddTotal;
            HDDUsed = (long)hddUsed;

            if (HDDTotal > 0 && HDDUsed > 0)
                HDDUsedPercent = (int)(((double)hddUsed / hddTotal) * 100);

            CPUUsedPercent = (int)(cpuUsed * 100);
        }

        public long MemoryTotal { get; set; }
        public long MemoryUsed { get; set; }
        public int MemoryUsedPercent { get; set; }

        public long HDDTotal { get; set; }
        public long HDDUsed { get; set; }
        public int HDDUsedPercent { get; set; }

        public int CPUUsedPercent { get; set; }
    }
}
