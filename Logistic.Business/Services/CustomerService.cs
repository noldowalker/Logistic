using System.Text.RegularExpressions;
using Domain.Interfaces;
using Domain.Models;
using Logistic.Application.BusinessServiceResults;

namespace Logistic.Application.Services;

public class CustomerService
{
    public ICustomersRepository CustomersRepository;

    public CustomerService(ICustomersRepository customersRepository)
    {
        CustomersRepository = customersRepository;
    }
    public GetListServiceResult<Customer> GetListOfCustomers()
    {
        var customers = CustomersRepository.GetList();
        
        return ConvertCustomersToResult(customers);
    }

    public bool RegisterNewCustomer(string name)
    {
        var isMatch = Regex.IsMatch(name, @"^[а-яА-Я\s]+$");

        if (!isMatch)
            return false;

        var newCustomer = new Customer()
        {
            name = name
        };
        
        CustomersRepository.Create(newCustomer);
        CustomersRepository.Save();
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