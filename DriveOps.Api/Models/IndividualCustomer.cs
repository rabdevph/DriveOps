namespace DriveOps.Api.Models;

public class IndividualCustomer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public Customer Customer { get; set; } = null!;
}
