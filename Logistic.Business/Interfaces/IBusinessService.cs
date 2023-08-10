using Domain.Interfaces;
using Logistic.Application.BusinessServiceResults;

namespace Logistic.Application;

public interface IBusinessService
{
    public List<WorkRecord> ActionRecords { get; set; }
    public bool IsLastActionSuccessful { get; } 
}