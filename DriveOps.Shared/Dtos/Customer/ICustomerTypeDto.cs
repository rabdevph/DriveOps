using DriveOps.Shared.Enums;

namespace DriveOps.Shared.Dtos.Customer;

public interface ICustomerTypeDto
{
    CustomerType Type { get; }
    CustomerIndividualDto? Individual { get; }
    CustomerCompanyDto? Company { get; }
}
