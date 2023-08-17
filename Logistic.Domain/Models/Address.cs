using System.ComponentModel.DataAnnotations.Schema;
using Domain.Attributes;

namespace Domain.Models;

[Table("addresses")]
[AutoGenerate]
public class Address : BaseModel
{
    public Address(string fullAddress)
    {
        FullAddress = fullAddress;
    }

    public string FullAddress { get; set; }
}