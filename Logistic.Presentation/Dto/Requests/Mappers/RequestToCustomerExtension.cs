using System.Text.Json;
using Domain.Models;
using Logistic.Application.BusinessModels;
using Logistic.Application.BusinessModels.Customer;

namespace Logistic.Dto.Requests.Mappers;

public static class RequestToCustomerExtension
{
    public static List<CustomerBusiness> TryToCustomers(this LogisticWebCreateOrUpdateRequest request)
    {
        var customers = new List<CustomerBusiness>();
        var data = request.Data;

        foreach (var record in data)
        {
            var customer = ConvertRecordToCustomer(record);
            
            if (customer != null)
                customers.Add(customer);
        }
        
        return customers;
    }

    private static CustomerBusiness? ConvertRecordToCustomer(object record)
    {
        
        string json = ((JsonElement) record).GetRawText();
        var customer = JsonSerializer.Deserialize<CustomerBusiness>(json);

        return customer;
    }
}