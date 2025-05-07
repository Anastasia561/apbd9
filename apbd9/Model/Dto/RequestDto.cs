using System.ComponentModel.DataAnnotations;

namespace apbd9.Model.Dto;

public class RequestDto
{
    [Required] [Range(1, int.MaxValue)] public int IdProduct { get; set; }
    [Required] [Range(1, int.MaxValue)] public int IdWarehouse { get; set; }
    [Required] [Range(1, int.MaxValue)] public int Amount { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
}