using Logistic.Application.Services;
using Logistic.Dto.Requests;
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

    [HttpGet(Name = nameof(Get))]
    public LogisticWebResponse Get()
    {
        var result = _customerService.GetListOfCustomers();
        
        return new LogisticWebResponse()
        {
            Data = result?.ListOfModels,
            Notification = result?.Notification,
            Error = result?.Error,
        }; 
    }
    
    [HttpPost(nameof(Create))]
    public async Task<LogisticWebResponse> Create(CreateCustomerForm form)
    {
        var result = await _customerService.RegisterNewCustomer(form.Name);
        
        if (result)
            return new LogisticWebResponse()
            {
                Notification = "Пользователь успешно добавлен"
            };  
        
        return new LogisticWebResponse()
        {
            Error = "Не удалось добавить пользователя"
        }; 
    }
    
    [HttpPost(nameof(ChangeCustomer))]
    public LogisticWebResponse ChangeCustomer(ChangeCustomerForm form)
    {
        var result = _customerService.UpdateCurrentCustomer(form.Id, form.NewName, form.Version);
        
        if (result)
            return new LogisticWebResponse()
            {
                Notification = "Имя пользователя успешно изменено"
            };  
        
        return new LogisticWebResponse()
        {
            Error = "Не удалось изменить имя пользователя"
        }; 
    }
}