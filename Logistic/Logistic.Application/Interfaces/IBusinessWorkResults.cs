using Domain.WorkResults;

namespace Logistic.Application.Interfaces;

public interface IBusinessActionMessageContainer : IActionMessageContainer
{
    public void AddInfrastructureResults(List<ActionMessage> results, bool isSuccessful);
}