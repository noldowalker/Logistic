namespace Logistic.Dto.Requests;

public class ChangeCustomerForm
{
    public long Id { get; set; }
    public string NewName { get; set; }
    public byte[] Version { get; set; }
}