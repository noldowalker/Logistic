using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Logistic.Controllers;
 
public class GenericControllerRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (!controller.ControllerType.IsGenericType) // Модель контроллера должна быть дженериком
            return;
        
        var genericType = controller.ControllerType.GenericTypeArguments[0];
        controller.ControllerName = genericType.Name; // Имя контроллера = дженерик типу
        if (controller.Selectors.Count > 0) // Если селекторы уже существуют код обновляет существующий селектор
        {
            var currentSelector = controller.Selectors[0];
            currentSelector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"/{genericType.Name}"));
        }
        else //  Если селекторов нет, то создается новый селектор (SelectorModel) с соответствующим маршрутом и добавляется к контроллеру.
        {
            controller.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"/{genericType.Name}"))
            });
        }
        
        // Другими словами весь этот код нужен чтобы для дженерик контроллеров роут начинался с genericType.Name
    }
}