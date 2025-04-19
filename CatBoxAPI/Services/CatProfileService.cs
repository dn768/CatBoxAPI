using CatBoxAPI.DB;
using CatBoxAPI.DB.Entities;
using CatBoxAPI.Models.CatProfile;
using Microsoft.EntityFrameworkCore;

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

    public async Task UpdateCatProfileAsync(CatProfileEditDTO catProfile)
    {
        var catProfileEntity = await catBoxDb.CatProfiles.FirstOrDefaultAsync(p => p.Id == catProfile.Id)
            ?? throw new UserFriendlyException($"Cat profile with id {catProfile.Id} not found.");
        
        catProfileEntity.Nickname = catProfile.Nickname;
        catProfileEntity.Weight = catProfile.Weight;
        catProfileEntity.PurrferedBoxSize = catProfile.GetBoxSize();
        
        await catBoxDb.SaveChangesAsync();
    }
}
