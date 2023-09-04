using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using Logistic.Application.Exceptions;
using Logistic.Application.Interfaces;
using Logistic.Application.WorkResult;

namespace Logistic.Application.Services;

public class CustomerService : IBusinessService<Customer>
{
    public IBusinessActionMessageContainer Results { get; set; }
    
    private IBaseModelsRepository<Customer> _customersRepository;
    private IBaseModelsRepository<Address> _addressesRepository;
    private AbstractValidator<Customer> _customerValidator;
    private AbstractValidator<Address> _addressValidator;

    public CustomerService(
        IBaseModelsRepository<Customer> customersRepository, 
        IBaseModelsRepository<Address> addressesRepository,
        AbstractValidator<Customer> customerValidator,
        AbstractValidator<Address> addressValidator,
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
            var validationResult = _customerValidator.Validate(
                customer, 
                options => options.IncludeRuleSets("Common", "Create"));

            if (!validationResult.IsValid)
            {
                Results.ConvertFromValidation(validationResult.Errors);
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
            var validationResult = _customerValidator.Validate(
                customer, 
                options => options.IncludeRuleSets("Common", "Update"));

            if (!validationResult.IsValid)
            {
                Results.ConvertFromValidation(validationResult.Errors);
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
    
    public async Task<BusinessResult<Customer>> DeleteCustomers(List<Customer> customers)
    {
        List<Customer> removedEntities = new List<Customer>();
        
        foreach (var customer in customers)
        {
            var searchCustomerResult = _customersRepository.Get(customer.Id);
            if (searchCustomerResult.Data.Count < 1)
            {
                Results.AddBusinessError($"Не найден пользователь с Id = {customer.Id}");
            }

            var customerForRemove = searchCustomerResult.Data.First();
            customerForRemove.Inactive = true;
            
            var validationResult = _customerValidator.Validate(
                customerForRemove, 
                options => options.IncludeRuleSets("Delete"));

            if (!validationResult.IsValid)
            {
                Results.ConvertFromValidation(validationResult.Errors);
                continue;
            }
            
            var result = await _customersRepository.Update(customerForRemove);
            if (result.IsSuccessful)
                removedEntities.AddRange(result.Data);
            
            Results.Messages.AddRange(result.Messages);
        }

        return new BusinessResult<Customer>(removedEntities, Results.Messages, !Results.IsBroken);
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
            var validationResult = _addressValidator.Validate(address, options => options.IncludeRuleSets("Common", "Create"));
            if (!validationResult.IsValid)
            {
                Results.ConvertFromValidation(validationResult.Errors);
                return null;
            }
            result = (await _addressesRepository.Create(address)).Data.First();
        }

        return result;
    }
}