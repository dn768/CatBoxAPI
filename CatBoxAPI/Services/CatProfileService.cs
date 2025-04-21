using CatBoxAPI.DB;
using CatBoxAPI.DB.Entities;
using CatBoxAPI.Enums;
using CatBoxAPI.Extensions;
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
            PurrferedBoxSize = catProfile.PurrferedBoxSize.GetBoxSize(),
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
        catProfileEntity.PurrferedBoxSize = catProfile.PurrferedBoxSize.GetBoxSize();
        
        await catBoxDb.SaveChangesAsync();
    }

    public async Task DeleteCatProfileAsync(Guid id)
    {
        var catProfileEntity = await catBoxDb.CatProfiles.FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new UserFriendlyException($"Cat profile with id {id} not found.");

        catBoxDb.CatProfiles.Remove(catProfileEntity);
        await catBoxDb.SaveChangesAsync();
    }

    public async Task<IEnumerable<CatProfileListItemDTO>> GetCatProfileListAsync(BoxSize? filterBySize, bool? sortByAge)
    {
        var query = catBoxDb.CatProfiles.AsQueryable();
        
        if (filterBySize.HasValue)
        {
            query = query.Where(p => p.PurrferedBoxSize == filterBySize);
        }
        
        if (sortByAge.HasValue)
        {
            query = sortByAge.Value ?
                query.OrderBy(p => p.Age) :
                query.OrderByDescending(p => p.Age);
        }

        var catProfiles = await query.ToListAsync();
        
        List<CatProfileListItemDTO> resultList = [];
        catProfiles.ForEach(p =>
        {
            resultList.Add(new()
            {
                Id = p.Id,
                Name = p.Name,
                Nickname = p.Nickname,
                Age = p.Age,
                Color = p.Color,
                Weight = p.Weight,
                Sex = p.Sex,
                PurrferedBoxSize = p.PurrferedBoxSize.ToString(),
            });
        });

        return resultList;
    }
}
