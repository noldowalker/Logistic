using Domain.Models;
using Domain.WorkResults;
using Logistic.Application.Services;
using Logistic.Dto.Requests;
using Logistic.Dto.Responses;
using Logistic.Infrastructure.Extensions;
using Logistic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Controllers;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class UsePresentationResultContainerAttribute : Attribute
{
    // Можете добавить дополнительные свойства или логику, если необходимо
}

[ApiController]
[Route("[controller]/[action]")]
[UsePresentationResultContainer]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly CustomerService _customerService;
    
    public IPresentationActionMessageContainer Resultses { get; }
    public CustomerController(ILogger<CustomerController> logger, CustomerService customerService, IPresentationActionMessageContainer resultses)
    {
        _logger = logger;
        _customerService = customerService;
        Resultses = resultses;
    }

    [HttpGet]
    public LogisticWebResponse GetAllCustomers()
    {
        var result = _customerService.GetListOfCustomers();
        var data = result.ConvertToObjectsList();

        return new LogisticWebResponse(data);
    }
    
    [HttpPost]
    public async Task<LogisticWebResponse> Create(LogisticWebRequestWithEntityList<Customer> form)
    {
        var customers = form.Data;
        var createdRecords = await _customerService.RegisterNewCustomers(customers);

        return new LogisticWebResponse(createdRecords.ConvertToObjectsList());
    }
    
    [HttpPost]
    public async Task<LogisticWebResponse> Change(LogisticWebRequestWithEntityList<Customer> form)
    {
        var customers = form.Data;
        var updatedRecords = await _customerService.UpdateCustomers(customers);

        return new LogisticWebResponse(updatedRecords.ConvertToObjectsList());
    }
}