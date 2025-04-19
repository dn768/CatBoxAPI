using CatBoxAPI.Models.CatProfile;

namespace CatBoxAPI.Services;

public interface ICatProfileService
{
    Task<Guid> CreateCatProfileAsync(CatProfileCreationDTO catProfile);
}
