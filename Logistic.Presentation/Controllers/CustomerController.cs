using Domain.Models;
using Domain.WorkResults;
using Logistic.Application.Services;
using Logistic.Dto.Requests;
using Logistic.Dto.Responses;
using Logistic.Infrastructure.Extensions;
using Logistic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly CustomerService _customerService;
    
    public IPresentationActionMessageContainer Results { get; }
    public CustomerController(ILogger<CustomerController> logger, CustomerService customerService, IPresentationActionMessageContainer results)
    {
        _logger = logger;
        _customerService = customerService;
        Results = results;
    }

    [HttpGet]
    public LogisticWebResponse GetAllCustomers()
    {
        var result = _customerService.GetListOfCustomers();
        var data = result.Data.ConvertToObjectsList();
        Results.AddBusinessResults(result.Messages, result.IsSuccessful);
        
        return new LogisticWebResponse(data, Results);
    }
    
    [HttpPost]
    public async Task<LogisticWebResponse> Create(LogisticWebRequestWithEntityList<Customer> form)
    {
        var customers = form.Data;
        var result = await _customerService.RegisterNewCustomers(customers);
        var data = result.Data.ConvertToObjectsList();
        Results.AddBusinessResults(result.Messages, result.IsSuccessful);
        
        return new LogisticWebResponse(data, Results);
    }
    
    [HttpPost]
    public async Task<LogisticWebResponse> Change(LogisticWebRequestWithEntityList<Customer> form)
    {
        var customers = form.Data;
        var result = await _customerService.UpdateCustomers(customers);
        var data = result.Data.ConvertToObjectsList();
        Results.AddBusinessResults(result.Messages, result.IsSuccessful);
        
        return new LogisticWebResponse(data, Results);
    }
}