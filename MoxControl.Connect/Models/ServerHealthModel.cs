namespace MoxControl.Connect.Models
{
    public class ServerHealthModel : BaseHealthModel
    {
        public ServerHealthModel(long memoryTotal, double memoryUsed, long hddTotal, double hddUsed, double cpuUsed)
            : base(memoryTotal, memoryUsed, hddTotal, hddUsed, cpuUsed)
        {

        }
    }
}
