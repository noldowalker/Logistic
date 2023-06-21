using Logistic.Application.Services;
using Logistic.Dto.Requests;
using Logistic.Dto.Requests.Mappers;
using Logistic.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private CustomerService _customerService;

    public CustomerController(ILogger<CustomerController> logger, CustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    [HttpGet()]
    public LogisticWebResponse Get()
    {
        var result = _customerService.GetListOfCustomers();
        
        return new LogisticWebResponse()
        {
            Data = result?.ListOfModels?.Cast<object>().ToList() ?? new List<object>(),
            Notification = result?.Notification,
            Errors = _customerService.ActionErrors,
        }; 
    }
    
    [HttpPost()]
    public async Task<LogisticWebResponse> Create(LogisticWebCreateOrUpdateRequest form)
    {
        var customers = form.TryToCustomers();
        await _customerService.RegisterNewCustomers(customers);
        
        // ToDo: переделать на BusinessServiceResult
        if (!_customerService.ActionErrors.Any())
            return new LogisticWebResponse()
            {
                Notification = "Пользователь успешно добавлен"
            };  
        
        return new LogisticWebResponse()
        {
            Errors = _customerService.ActionErrors
        }; 
    }
    
    [HttpPost()]
    public LogisticWebResponse Change(LogisticWebCreateOrUpdateRequest form)
    {
        var customers = form.TryToCustomers();
        var result = _customerService.UpdateCustomers(customers);
        // ToDo: переделать на BusinessServiceResult
        if (!_customerService.ActionErrors.Any())
            return new LogisticWebResponse()
            {
                Notification = "Имя пользователя успешно изменено"
            };

        return LogisticWebResponse.BadResult(_customerService.ActionErrors);
    }
}