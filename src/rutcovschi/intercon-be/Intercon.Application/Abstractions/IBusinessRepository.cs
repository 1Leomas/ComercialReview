using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;

namespace Intercon.Application.Abstractions;

public interface IBusinessRepository
{
    Task<Business?> GetBusinessByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Business>> GetAllBusinessesAsync(CancellationToken cancellationToken);
    Task<int> CreateBusinessAsync(Business newBusiness, CancellationToken cancellationToken);
    Task<Business?> UpdateBusinessAsync(int id, EditBusinessRequest newBusinessData, int? logoId, CancellationToken cancellationToken);
    Task<bool> BusinessExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> UserHasBusinessAsync(int userId, CancellationToken cancellationToken);
    Task<int?> GetBusinessLogoIdAsync(int businessId, CancellationToken cancellationToken);
    Task<bool> UserOwnsBusinessAsync(int userId, int businessId, CancellationToken cancellationToken);
}