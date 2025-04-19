using CatBoxAPI.Models.CatProfile;

namespace CatBoxAPI.Services;

public interface ICatProfileService
{
    Task<Guid> CreateCatProfileAsync(CatProfileCreationDTO catProfile);
    Task UpdateCatProfileAsync(CatProfileEditDTO catProfile);
    Task DeleteCatProfileAsync(Guid id);
}
