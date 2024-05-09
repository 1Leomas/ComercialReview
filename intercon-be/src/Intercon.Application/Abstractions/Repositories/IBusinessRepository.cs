using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;
using Intercon.Domain.Pagination;

namespace Intercon.Application.Abstractions.Repositories;

public interface IBusinessRepository
{
    Task<Business?> GetBusinessByIdAsync(int id, CancellationToken cancellationToken);
    Task<Business?> GetBusinessByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<IEnumerable<Business>> GetAllBusinessesAsync(CancellationToken cancellationToken);
    Task<PaginatedList<Business>> GetPaginatedBusinessesAsync(BusinessParameters parameters, CancellationToken cancellationToken);
    Task<int> CreateBusinessAsync(Business newBusiness, CancellationToken cancellationToken);
    Task<Business?> UpdateBusinessAsync(int id, EditBusinessRequest newBusinessData, int? logoId, CancellationToken cancellationToken);
    Task<bool> BusinessExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> UserHasBusinessAsync(int userId, CancellationToken cancellationToken);
    Task<int?> GetBusinessLogoIdAsync(int businessId, CancellationToken cancellationToken);
    Task<bool> UserOwnsBusinessAsync(int userId, int businessId, CancellationToken cancellationToken);
}