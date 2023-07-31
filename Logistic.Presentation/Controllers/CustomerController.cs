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
    public ObjectResult GetAllCustomers()
    {
        var result = _customerService.GetListOfCustomers();

        var response = new LogisticWebResponse()
        {
            Data = result?.ListOfModels?.Cast<object>().ToList() ?? new List<object>(),
            Notification = result?.Notification,
            Errors = _customerService.ActionErrors,
        };

        return response.AsObjectResult();
    }
    
    [HttpPost]
    public async Task<ObjectResult> Create(LogisticWebRequestWithEntityList<CustomerBusiness> form)
    {
        var customers = form.Data;
        await _customerService.RegisterNewCustomers(customers);
        
        if (!_customerService.ActionErrors.Any())
            return new LogisticWebResponse()
            {
                Notification = "Пользователь успешно добавлен"
            }.AsObjectResult();  
        
        return new LogisticWebResponse()
        {
            Errors = _customerService.ActionErrors
        }.AsObjectResult(); 
    }
    
    [HttpPost]
    public async Task<ObjectResult> Change(LogisticWebRequestWithEntityList<CustomerBusiness> form)
    {
        var customers = form.Data;
        await _customerService.UpdateCustomers(customers);

        if (!_customerService.ActionErrors.Any())
            return new LogisticWebResponse()
            {
                Notification = "Имя пользователя успешно изменено"
            }.AsObjectResult();

        return LogisticWebResponse.BadResult(_customerService.ActionErrors).AsObjectResult();
    }
}