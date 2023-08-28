using System.ComponentModel;

namespace Logistic.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class OrderAttribute : DefaultValueAttribute
{
    public int Order { get; }
    public OrderAttribute(int value) : base(value)
    {
        Order = value;
    }
}