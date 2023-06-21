using Domain.Interfaces;
using Domain.Models;
using Logistic.Application.BusinessModels;
using Logistic.Application.BusinessModels.Customer;
using Logistic.Application.BusinessServiceResults;

namespace Logistic.Application.Services;

public class CustomerService
{
    public List<string> ActionErrors { get; set; }
    
    private ICustomersRepository CustomersRepository;
    private DataBaseContext _db;
    
    public CustomerService(ICustomersRepository customersRepository, DataBaseContext db)
    {
        CustomersRepository = customersRepository;
        _db = db;
    }
    public GetListServiceResult<Customer> GetListOfCustomers()
    {
        var customers = CustomersRepository.GetList();
        
        return ConvertCustomersToResult(customers);
    }

    public async Task RegisterNewCustomers(List<CustomerBusiness> customers)
    {
        ActionErrors = new List<string>();
        foreach (var customerBusiness in customers)
        {
            customerBusiness.ValidateForCreate();
            if (customerBusiness.ValidationErrors.Any())
            {
                var error = $"Не удалось создать {customerBusiness.name} из-за ошибок валидации:" + String.Join(";", customerBusiness.ValidationErrors);
                ActionErrors.Add(error);
                continue;
            }

            var customer = customerBusiness.MapToDomain();
            CustomersRepository.Create(customer);
        }
        
        await CustomersRepository.SaveAsync();
    }

    public async Task UpdateCustomers(List<CustomerBusiness> customers)
    {
        ActionErrors = new List<string>();
        foreach (var customerBusiness in customers)
        {
            customerBusiness.ValidateForUpdate();
            if (customerBusiness.ValidationErrors.Any())
            {
                var error = $"не удалось изменить {customerBusiness.name} из-за ошибок валидации: " + String.Join(";", customerBusiness.ValidationErrors);
                ActionErrors.Add(error);
                continue;
            }

            var customer = customerBusiness.MapToDomain();
            var customerForEdit = CustomersRepository.Get(customer.id);
            if (customerForEdit == null)
            {
                ActionErrors.Add($"Пользователя с id {customerBusiness.id} не существует в системе");
            }

            customerForEdit.name = customerBusiness.name;
        
            CustomersRepository.Update(customerForEdit);
        }
        
        await CustomersRepository.SaveAsync();
    }

    private GetListServiceResult<Customer> ConvertCustomersToResult(IEnumerable<Customer> customers)
    {
        var result = new GetListServiceResult<Customer>();
        var customersList = customers.ToList();
        var isAnyCustomers = customersList.Any();

        if (!isAnyCustomers)
        {
            result.Notification = "Клиенты не найдены.";
        }
        else
        {
            result.ListOfModels = customersList;
        }

        return result;
    }
}