using System.Text.RegularExpressions;

namespace Logistic.Application.BusinessModels.Customer;

public partial class CustomerBusiness
{
    public override void ValidateForCreate()
    {
        base.ValidateForCreate();
        CheckName();
    }

    private void CheckName()
    {
        var isMatch = Regex.IsMatch(name, @"^[а-яА-Я\sa-zA-z]+$");

        if (!isMatch)
            ValidationErrors.Add("Имя может включать в себя только кириллические или латинские символы.");
    }
}