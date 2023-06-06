using System.ComponentModel.DataAnnotations.Schema;
using Domain.Attributes;

namespace Domain.Models;

[Table("delivery_points")]
[AutoGenerate]
public class DeliveryPoint : BaseModel
{
    public Address Address { get; set; }
    public string Name { get; set; }
}