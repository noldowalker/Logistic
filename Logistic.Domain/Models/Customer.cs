using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Customer: BaseModel
{
    [Required]
    public string ? Name { get; set; }

    public Address? Address { get; set; }
}