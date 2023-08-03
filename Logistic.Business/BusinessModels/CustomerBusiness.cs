using Domain.Models;

namespace Logistic.Application.BusinessModels;

//ToDo: хранить в отдельной папочке
public class CustomerBusiness : BaseModelBusiness
{
    public string? name { get; set; }
    
    public Address? Address { get; set; }
}