using Domain.Models;
using Domain.WorkResults;
using Logistic.Application.Interfaces;

namespace Logistic.Application;

public interface IBusinessService<T> where T : BaseModel
{
    public IBusinessActionMessageContainer Results { get; set; }
}