using Domain.WorkResults;
using Logistic.Application.Exceptions;
using Logistic.Application.Interfaces;
using Logistic.Infrastructure.Exceptions;

namespace Logistic.Interfaces;

public interface IPresentationActionMessageContainer: IActionMessageContainer
{
    public int GetStatusCode();
    public void AddBusinessResults(List<ResultMessage> messages, bool isSuccessful);
}