namespace AssemblyService.DTOs
{
    public class AssemblesForWorkerDTO
    {
        public int assignee_id { get; set; }
        public int vehicle_id { get; set; }
        public int NIC { get; set; }
        public DateOnly date { get; set; }
        public bool isCompleted { get; set; }
        public string? attachment_path { get; set; }
    }
}
