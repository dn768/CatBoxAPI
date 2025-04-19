using CatBoxAPI.Models.CatProfile;

namespace CatBoxAPI.Services;

public class CatProfileService : ICatProfileService
{
    public async Task<Guid> CreateCatProfileAsync(CatProfileCreationDTO catProfile)
    {
        // TODO: Use EF Core here
        return new Guid();
    }
}
