using Domain.Interfaces;
using Domain.Models;
using Logistic.Application.Exceptions;
using Logistic.Application.Interfaces;
using Logistic.Application.WorkResult;

namespace Logistic.Application.Services;

public class CustomerService : IBusinessService<Customer>
{
    public IBusinessActionMessageContainer Results { get; set; }
    
    private IBaseModelsRepository<Customer> _customersRepository;
    private IBaseModelsRepository<Address> _addressesRepository;
    private IValidatable<Customer> _customerValidator;
    private IValidatable<Address> _addressValidator;
    
    public CustomerService(
        IBaseModelsRepository<Customer> customersRepository, 
        IBaseModelsRepository<Address> addressesRepository,
        IValidatable<Customer> customerValidator,
        IValidatable<Address> addressValidator, 
        IBusinessActionMessageContainer resultses)
    {
        _customersRepository = customersRepository;
        _customerValidator = customerValidator;
        _addressValidator = addressValidator;
        _addressesRepository = addressesRepository;
        Results = resultses;
    }
    
    public BusinessResult<Customer> GetListOfCustomers()
    {
        var actionResult = _customersRepository.GetList();
        
        return new BusinessResult<Customer>(actionResult);
    }

    public async Task<BusinessResult<Customer>> RegisterNewCustomers(List<Customer> customers)
    {
        var createdEntities = new List<Customer>();
        foreach (var customer in customers)
        {
            if (!_customerValidator.IsValidForCreate(customer))
            {
                continue;
            }
            
            if (customer.Address != null)
                customer.Address = await GetOrCreateAddress(customer.Address);
            
            var result = await _customersRepository.Create(customer);
            if (result.IsSuccessful)
                createdEntities.AddRange(result.Data);
            
            Results.Messages.AddRange(result.Messages);
        }

        return new BusinessResult<Customer>(createdEntities, Results.Messages, !Results.IsBroken);
    }

    public async Task<BusinessResult<Customer>> UpdateCustomers(List<Customer> customers)
    {
        List<Customer> updatedEntities = new List<Customer>();
        
        foreach (var customer in customers)
        {
            if (!_customerValidator.IsValidForUpdate(customer))
            {
                var error = $"не удалось изменить {customer.Name} из-за ошибок валидации: " + String.Join(";", _customerValidator.Results);
                Results.AddError(new BusinessError(error));
                continue;
            }

            if (customer.Address != null)
                customer.Address = await GetOrCreateAddress(customer.Address);
            
            var result = await _customersRepository.Update(customer);
            if (result.IsSuccessful)
                updatedEntities.AddRange(result.Data);
            
            Results.Messages.AddRange(result.Messages);
        }

        return new BusinessResult<Customer>(updatedEntities, Results.Messages, !Results.IsBroken);
    }

    private async Task<Address?> GetOrCreateAddress(Address address)
    {
        Address? result;
        if (address.Id > 0)
        {
            result = _addressesRepository.Get(address.Id).Data.First();
        }
        else
        {
            if (!_addressValidator.IsValidForCreate(address))
            {
                var error = $"не удалось создать вложенный адрес из-за ошибок валидации: " + String.Join(";", _customerValidator.Results);
                Results.AddError(new BusinessError(error));
                return null;
            }
            result = (await _addressesRepository.Create(address)).Data.First();
        }

        return result;
    }
}