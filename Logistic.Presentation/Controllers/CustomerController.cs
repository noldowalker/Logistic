using Logistic.Application.Services;
using Logistic.Dto.Requests;
using Logistic.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Controllers;

[ApiController]
[Route("[controller]")]
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
    
    [HttpPost(Name = nameof(Create))]
    public LogisticWebResponse Create(CreateCustomerForm form)
    {
        var result = _customerService.RegisterNewCustomer(form.Name);
        
        if (result)
            return new LogisticWebResponse()
            {
                Notification = "Пользователь успешно добавлен"
            };  
        
        return new LogisticWebResponse()
        {
            Error = "Ну удалось добавить пользователя"
        }; 
    }
}