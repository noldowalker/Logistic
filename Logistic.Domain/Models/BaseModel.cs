using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class BaseModel
{
    [Key, Required]
    public long id { get; set; }

    public bool inactive { get; set; } = false;
}