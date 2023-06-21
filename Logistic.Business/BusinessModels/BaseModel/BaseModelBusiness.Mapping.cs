namespace Logistic.Application.BusinessModels.BaseModel;

public partial class BaseModelBusiness : IDomainMappable<Domain.Models.BaseModel>
{
    public virtual Domain.Models.BaseModel MapToDomain()
    {
        return new Domain.Models.BaseModel()
        {
            id = id ?? 0,
            inactive = inactive ?? false
        };
    }
}