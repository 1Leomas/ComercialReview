using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;

namespace Intercon.Application.Abstractions;

public interface IBusinessRepository
{
    Task<Business?> GetBusinessByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Business>> GetAllBusinessesAsync(CancellationToken cancellationToken);
    Task<int> CreateBusinessAsync(Business newBusiness, CancellationToken cancellationToken);
    Task<Business?> UpdateBusinessAsync(int id, EditBusinessDto newBusinessData, CancellationToken cancellationToken);
    Task<bool> BusinessExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> UserHasBusinessAsync(int userId, CancellationToken cancellationToken);
    Task SetBusinessLogoIdAsync(int businessId, int logoId, CancellationToken cancellationToken);
}