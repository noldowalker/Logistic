using Domain.Interfaces;
using Logistic.Application.Services;
using Logistic.Dto.Requests;
using Logistic.Dto.Responses;
using Logistic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CustomerBusiness = Logistic.Application.BusinessModels.CustomerBusiness;

namespace Logistic.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly CustomerService _customerService;

    public CustomerController(
        ILogger<CustomerController> logger,
        CustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    [HttpGet]
    public LogisticWebResponse GetAllCustomers()
    {
        var result = _customerService.GetListOfCustomers();

        var response = new LogisticWebResponse()
        {
            Data = result?.ListOfModels?.Cast<object>().ToList() ?? new List<object>(),
            Records = _customerService.ActionRecords
        };

        return response;
    }
    
    [HttpPost]
    public async Task<LogisticWebResponse> Create(LogisticWebRequestWithEntityList<CustomerBusiness> form)
    {
        var customers = form.Data;
        await _customerService.RegisterNewCustomers(customers);

        var response= new LogisticWebResponse();
        response.Records.AddRange(_customerService.ActionRecords);
        
        if (!_customerService.IsLastActionSuccessful)
            response.Records.Add(WorkRecord.CreateNotification("Пользователь успешно добавлен!"));

        return response;
    }
    
    [HttpPost]
    public async Task<ObjectResult> Change(LogisticWebRequestWithEntityList<CustomerBusiness> form)
    {
        var customers = form.Data;
        await _customerService.UpdateCustomers(customers);

        return (!_customerService.IsLastActionSuccessful)
            ?  LogisticWebResponse
                .CreateWithNotification("Пользователь успешно изменен", _customerService.ActionRecords)
                .AsObjectResult()
            :  LogisticWebResponse.BadResult(_customerService.ActionRecords).AsObjectResult(); 
    }
}