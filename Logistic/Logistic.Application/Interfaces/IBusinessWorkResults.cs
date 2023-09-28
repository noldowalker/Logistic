using Domain.WorkResults;
using FluentValidation.Results;

namespace Logistic.Application.Interfaces;

public interface IBusinessActionMessageContainer : IActionMessageContainer
{
    public void AddInfrastructureResults(List<ResultMessage> results, bool isSuccessful);

    public void ConvertFromValidation(List<ValidationFailure> validationFailures);
    public void AddBusinessError(string errorUserText);
}