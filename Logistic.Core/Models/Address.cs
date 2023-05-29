using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("addresses")]
public class Address : BaseModel
{
    public string FullAddress { get; set; }
}