using Domain.Models;
using Domain.WorkResults;
using Logistic.Application.Services;
using Logistic.Dto.Requests;
using Logistic.Dto.Responses;
using Logistic.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly CustomerService _customerService;
    public IWorkResult Results { get; }
    public CustomerController(ILogger<CustomerController> logger, CustomerService customerService, IWorkResult results)
    {
        _logger = logger;
        _customerService = customerService;
        Results = results;
    }

    [HttpGet]
    public LogisticWebResponse GetAllCustomers()
    {
        var result = _customerService.GetListOfCustomers();
        var data = result.ConvertToObjectsList();

        return new LogisticWebResponse(data, Results.Messages);;
    }
    
    [HttpPost]
    public async Task<LogisticWebResponse> Create(LogisticWebRequestWithEntityList<Customer> form)
    {
        var customers = form.Data;
        await _customerService.RegisterNewCustomers(customers);

        return new LogisticWebResponse(Results.Messages);
    }
    
    [HttpPost]
    public async Task<LogisticWebResponse> Change(LogisticWebRequestWithEntityList<Customer> form)
    {
        var customers = form.Data;
        var updatedRecords = await _customerService.UpdateCustomers(customers);

        return new LogisticWebResponse(
            updatedRecords.ConvertToObjectsList(),
            Results.Messages
        );;
    }
}