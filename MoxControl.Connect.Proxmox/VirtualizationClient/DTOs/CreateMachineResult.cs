namespace MoxControl.Connect.Proxmox.VirtualizationClient.DTOs
{
    public class CreateMachineResult
    {
        public CreateMachineResult(string uniqueTaskId, int vmId)
        {
            UniqueTaskId = uniqueTaskId;
            VmId = vmId;
        }

        public string UniqueTaskId { get; set; }
        public int VmId { get; set; }
    }
}
