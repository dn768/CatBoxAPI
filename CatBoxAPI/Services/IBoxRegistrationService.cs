using CatBoxAPI.Models.BoxRegistration;

namespace CatBoxAPI.Services;

public interface IBoxRegistrationService
{
    Task<Guid> CreateAsync(BoxRegistrationCreationDTO boxRegistration);
    Task<Guid> UpdateAsync(BoxRegistrationEditDTO boxRegistration);
}
