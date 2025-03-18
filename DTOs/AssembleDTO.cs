namespace AssemblyService.DTOs
{
    public class AssembleDTO
    {
        public int assignee_id { get; set; }
        public string? assignee_first_name { get; set; }
        public string? assignee_last_name { get; set; }
        public int vehicle_id { get; set; }
        public string? model { get; set; }
        public string? color { get; set; }
        public string? engine { get; set; }
        public int NIC { get; set; }
        public string? WorkerName { get; set; }
        public string? job_role { get; set; }
        public DateOnly date { get; set; }
        public bool isCompleted { get; set; }
        public string? attachment { get; set; }
    }
}
