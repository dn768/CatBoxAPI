using CatBoxAPI.Models.BoxRegistration;
using System.ComponentModel;

namespace CatBoxAPI.Services;

public interface IBoxRegistrationService
{
    Task<Guid> CreateAsync(BoxRegistrationCreationDTO boxRegistration);
    Task<Guid> UpdateAsync(BoxRegistrationEditDTO boxRegistration);
    Task<IEnumerable<BoxRegistrationListItemDTO>> GetBoxRegistrationListAsync(bool filterByApproval, bool? approvalFilter, ListSortDirection? sortByBoxSize);
    Task SaveRegistrationApproval(Guid registrationId, bool isApproved, string? decisionReason);
}
