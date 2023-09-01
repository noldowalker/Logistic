using Domain.WorkResults;
using FluentValidation.Results;

namespace Logistic.Application.Interfaces;

public interface IBusinessActionMessageContainer : IActionMessageContainer
{
    public void AddInfrastructureResults(List<ActionMessage> results, bool isSuccessful);

    public void ConvertFromValidation(List<ValidationFailure> validationFailures);
}