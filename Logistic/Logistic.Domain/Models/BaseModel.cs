using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class BaseModel
{
    [Key, Required]
    public long Id { get; set; }

    public bool Inactive { get; set; } = false;
}