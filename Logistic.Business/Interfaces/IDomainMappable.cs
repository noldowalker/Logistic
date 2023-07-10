using Domain.Models;
using Logistic.Application.BusinessModels;

namespace Logistic.Application;

public interface IDomainMappable<TDomain, TBusiness> 
    where TDomain : BaseModel 
    where TBusiness : BaseModelBusiness
{
    public TDomain MapToDomain(TBusiness entity);
    public TBusiness MapFromDomain(TDomain entity);
}