using Domain.Models;

namespace Logistic.Application;

public interface IDomainMappable<T> where T : BaseModel
{
    public T MapToDomain();
}