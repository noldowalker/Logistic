using Domain.Models;
using Logistic.Application.BusinessModels;

namespace Logistic.Application.Mappers;

public class BaseModelBusinessMapper : IDomainMappable<BaseModel, BaseModelBusiness>
{
    public BaseModel MapToDomain(BaseModelBusiness entity)
    {
        return new BaseModel()
        {
            id = entity.id ?? 0,
            inactive = entity.inactive ?? false
        };
    }

    public BaseModelBusiness MapFromDomain(BaseModel entity)
    {
        return new BaseModelBusiness()
        {
            id = entity.id,
            inactive = entity.inactive
        };
    }
}