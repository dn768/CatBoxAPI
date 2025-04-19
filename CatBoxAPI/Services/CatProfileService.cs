using CatBoxAPI.DB;
using CatBoxAPI.DB.Entities;
using CatBoxAPI.Models.CatProfile;

namespace CatBoxAPI.Services;

public class CatProfileService(CatBoxContext catBoxDb) : ICatProfileService
{
    public async Task<Guid> CreateCatProfileAsync(CatProfileCreationDTO catProfile)
    {
        // TODO: Consider using AutoMapper
        var id = Guid.NewGuid();
        var catProfileEntity = new CatProfileEntity()
        {
            Id = id,
            Name = catProfile.Name,
            Nickname = catProfile.Nickname,
            Age = catProfile.Age,
            Color = catProfile.Color,
            Sex = catProfile.Sex,
            Weight = catProfile.Weight,
            PurrferedBoxSize = catProfile.GetBoxSize(),
        };

        await catBoxDb.CatProfiles.AddAsync(catProfileEntity);
        await catBoxDb.SaveChangesAsync();

        return id;
    }
}
