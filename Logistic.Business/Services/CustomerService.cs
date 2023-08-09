using Domain.Interfaces;
using Domain.Models;
using Logistic.Application.BusinessModels;
using Logistic.Application.BusinessServiceResults;
using CustomerBusiness = Logistic.Application.BusinessModels.CustomerBusiness;

namespace Logistic.Application.Services;

public class CustomerService
{
    public List<string> ActionErrors { get; set; }
    
    private ICustomersRepository _customersRepository;
    private IAddressesRepository _addressesRepository;
    private IValidatable<CustomerBusiness> _customerValidator;
    private IDomainMappable<Customer,CustomerBusiness> _customerMapper;
    
    public CustomerService(
        ICustomersRepository customersRepository, 
        IValidatable<CustomerBusiness> customerValidator,
        IDomainMappable<Customer,CustomerBusiness> customerMapper)
    {
        _customersRepository = customersRepository;
        _customerValidator = customerValidator;
        _customerMapper = customerMapper;
    }
    
    public GetListServiceResult<CustomerBusiness> GetListOfCustomers()
    {
        var customers = _customersRepository.GetList();
        
        return ConvertCustomersToResult(customers);
    }

    public async Task RegisterNewCustomers(List<CustomerBusiness> customers)
    {
        ActionErrors = new List<string>();
        foreach (var customerBusiness in customers)
        {
            _customerValidator.ValidateForCreate(customerBusiness);
            if (_customerValidator.ValidationErrors.Any())
            {
                var error = $"Не удалось создать {customerBusiness.name} из-за ошибок валидации:" + String.Join(";", _customerValidator.ValidationErrors);
                ActionErrors.Add(error);
                continue;
            }

            
            var customer = _customerMapper.MapToDomain(customerBusiness);
            if (customer.Address != null)
            {
                customer.Address = _addressesRepository.Get(customer.Address.id);
            }
            
            _customersRepository.Create(customer);
        }
        
        await _customersRepository.SaveAsync();
    }

    public async Task UpdateCustomers(List<CustomerBusiness> customers)
    {
        ActionErrors = new List<string>();
        foreach (var customerBusiness in customers)
        {
            _customerValidator.ValidateForUpdate(customerBusiness);
            if (_customerValidator.ValidationErrors.Any())
            {
                var error = $"не удалось изменить {customerBusiness.name} из-за ошибок валидации: " + String.Join(";", _customerValidator.ValidationErrors);
                ActionErrors.Add(error);
                continue;
            }

            var customer = _customerMapper.MapToDomain(customerBusiness);

            _customersRepository.Update(customer);
        }
        
        await _customersRepository.SaveAsync();
    }
    
    private GetListServiceResult<CustomerBusiness> ConvertCustomersToResult(IEnumerable<Customer> customers)
    {
        var result = new GetListServiceResult<CustomerBusiness>();
        var customersList = customers.ToList();
        var isAnyCustomers = customersList.Any();

        if (!isAnyCustomers)
        {
            result.Notification = "Клиенты не найдены.";
            return result;
        }
        
        foreach (var customer in customersList)
        {
            result.ListOfModels.Add(_customerMapper.MapFromDomain(customer));
        }
        

        return result;
    }
}