using DriveOps.Shared.Dtos.Customer;
using DriveOps.Shared.Enums;

namespace DriveOps.Api.Interfaces;

public interface ICustomerService
{
    Task<CustomerPaginatedResultDto<CustomerDetailsDto>> GetAllAsync(CustomerType? type, int page, int pageSize);
    Task<CustomerDetailsDto?> GetByIdAsync(int id, bool onlyCurrent);
    Task<CustomerDetailsDto> CreateAsync(CustomerCreateDto dto);
    Task<CustomerDetailsDto?> UpdateDetailsAsync(int id, CustomerDetailsUpdateDto dto);
    Task<CustomerDetailsDto?> UpdateStatusAsync(int id, CustomerStatusUpdateDto dto);
}
