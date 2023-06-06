using System.Text;
using System.Text.RegularExpressions;
using Domain.Interfaces;
using Domain.Models;
using Logistic.Application.BusinessServiceResults;

namespace Logistic.Application.Services;

public class CustomerService
{
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

    public async Task<bool> RegisterNewCustomer(string name)
    {
        var isMatch = Regex.IsMatch(name, @"^[а-яА-Я\sa-zA-z]+$");

        if (!isMatch)
            return false;

        var newCustomer = new Customer()
        {
            name = name,
        };
        
        CustomersRepository.Create(newCustomer);
        await CustomersRepository.SaveAsync();
        return true;
    }

    public bool UpdateCurrentCustomer(long id, string newName, byte[] version)
    {
        var customerForEdit = CustomersRepository.Get(id);
        if (customerForEdit == null)
            return false;
        
        customerForEdit.name = newName;
        CustomersRepository.Update(customerForEdit);
        CustomersRepository.SaveAsync();
        
        return true;
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