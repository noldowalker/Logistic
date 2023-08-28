using System.ComponentModel.DataAnnotations.Schema;
using Domain.Attributes;

namespace Domain.Models;

[Table("addresses")]
[AutoGenerate]
public class Address : BaseModel
{
    public string FullAddress { get; set; }
}