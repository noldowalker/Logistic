namespace Logistic.Application;

public interface IValidatable
{
    public List<string> ValidationErrors { get; set; }
    
    public void ValidateForCreate();
    public void ValidateForUpdate();
    public void ValidateForDelete();
}