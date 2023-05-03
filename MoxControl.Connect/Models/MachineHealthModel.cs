namespace MoxControl.Connect.Models
{
    public class MachineHealthModel : BaseHealthModel
    {
        public MachineHealthModel(long memoryTotal, double memoryUsed, long hddTotal, double hddUsed, double cpuUsed)
            : base(memoryTotal, memoryUsed, hddTotal, hddUsed, cpuUsed)
        {

        }
    }
}
