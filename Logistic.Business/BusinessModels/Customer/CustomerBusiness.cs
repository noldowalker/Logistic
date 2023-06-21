using Logistic.Application.BusinessModels.BaseModel;

namespace Logistic.Application.BusinessModels.Customer;

//ToDo: хранить в отдельной папочке
public partial class CustomerBusiness : BaseModelBusiness, IValidatable
{
    public string ? name { get; set; }
}