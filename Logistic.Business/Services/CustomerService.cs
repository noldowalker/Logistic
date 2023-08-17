using Domain.Interfaces;
using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application.Services;

public class CustomerService : IBusinessService<Customer>
{
    public IWorkResult Results { get; set; }
    
    private IBaseModelsRepository<Customer> _customersRepository;
    private IBaseModelsRepository<Address> _addressesRepository;
    private IValidatable<Customer> _customerValidator;
    private IValidatable<Address> _addressValidator;
    
    public CustomerService(
        IBaseModelsRepository<Customer> customersRepository, 
        IBaseModelsRepository<Address> addressesRepository,
        IValidatable<Customer> customerValidator,
        IValidatable<Address> addressValidator, 
        IWorkResult results)
    {
        _customersRepository = customersRepository;
        _customerValidator = customerValidator;
        _addressValidator = addressValidator;
        _addressesRepository = addressesRepository;
        Results = results;
    }
    
    public List<Customer>? GetListOfCustomers()
    {
        var customers = _customersRepository.GetList();
        return customers.ToList();
    }

    public async Task RegisterNewCustomers(List<Customer> customers)
    {
        foreach (var customer in customers)
        {
            if (!_customerValidator.IsValidForCreate(customer))
            {
                continue;
            }
            
            if (customer.Address != null)
                customer.Address = await GetOrCreateAddress(customer.Address);
            
            await _customersRepository.Create(customer);
        }
    }

    public async Task<List<Customer>?> UpdateCustomers(List<Customer> customers)
    {
        List<Customer> result = new List<Customer>();
        
        foreach (var customer in customers)
        {
            if (!_customerValidator.IsValidForUpdate(customer))
            {
                var error = $"не удалось изменить {customer.Name} из-за ошибок валидации: " + String.Join(";", _customerValidator.Result);
                Results.AddBusinessErrorMessage(error);
                continue;
            }

            if (customer.Address != null)
                customer.Address = await GetOrCreateAddress(customer.Address);
            
            await _customersRepository.Update(customer);
            
            result.Add(customer);
        }
        
        await _customersRepository.SaveAsync();

        return result;
    }

    private async Task<Address?> GetOrCreateAddress(Address address)
    {
        Address? result;
        if (address.id > 0)
        {
            result = _addressesRepository.Get(address.id);
        }
        else
        {
            if (!_addressValidator.IsValidForCreate(address))
            {
                var error = $"не удалось создать вложенный адрес из-за ошибок валидации: " + String.Join(";", _customerValidator.Result);
                Results.AddBusinessErrorMessage(error);
                return null;
            }
            result = await _addressesRepository.Create(address);
            await _addressesRepository.SaveAsync();
        }

        return result;
    }
}