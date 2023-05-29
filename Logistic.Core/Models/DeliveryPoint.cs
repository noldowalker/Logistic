using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("delivery_points")]
public class DeliveryPoint : BaseModel
{
    public Address Address { get; set; }
    public string Name { get; set; }
}