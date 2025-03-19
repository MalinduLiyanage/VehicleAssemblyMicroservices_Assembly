using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssemblyService.Models
{
    [Table("assembles")]
    [PrimaryKey(nameof(vehicle_id), nameof(NIC))]
    public class AssembleModel
    {
        public int assignee_id { get; set; }
        public int vehicle_id { get; set; }
        public int NIC { get; set; }
        public DateOnly date { get; set; }
        public bool isCompleted { get; set; }
        public string? attachment_path { get; set; }
    }
}
