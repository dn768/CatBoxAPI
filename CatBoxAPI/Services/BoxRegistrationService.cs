using CatBoxAPI.DB;
using CatBoxAPI.DB.Entities;
using CatBoxAPI.Enums;
using CatBoxAPI.Extensions;
using CatBoxAPI.Models.BoxRegistration;
using Microsoft.EntityFrameworkCore;

namespace CatBoxAPI.Services;

public class BoxRegistrationService(CatBoxContext catBoxDb) : IBoxRegistrationService
{
    private readonly CatBoxContext _catBoxDb = catBoxDb;
    
    public async Task<Guid> CreateAsync(BoxRegistrationCreationDTO boxRegistration)
    {
        // TODO: Save BoxRegistration without getting the CatProfileEntity first. Return the error on foreign key violation
        var cat = await _catBoxDb.CatProfiles.FindAsync(boxRegistration.CatId);

        if (cat == null)
            throw new UserFriendlyException($"Cat profile with id {boxRegistration.CatId} not found.");

        var boxRegistrationEntity = new BoxRegistration()
        {
            Id = Guid.NewGuid(),
            Cat = cat,
            BoxType = boxRegistration.BoxType,
            BoxSize = boxRegistration.BoxSize.GetBoxSize(),
            SpecialFeatures = boxRegistration.SpecialFeatures,
            IsApproved = false,
            CreatedAt = DateTime.UtcNow,
        };

        await _catBoxDb.AddAsync(boxRegistrationEntity);
        await _catBoxDb.SaveChangesAsync();

        return boxRegistrationEntity.Id;
    }

    public async Task<Guid> UpdateAsync(BoxRegistrationEditDTO boxRegistration)
    {
        var registration = await _catBoxDb.BoxRegistrations.FindAsync(boxRegistration.Id);

        if (registration == null)
            throw new UserFriendlyException($"Box registration with id {boxRegistration.Id} not found.");
        
        registration.BoxType = boxRegistration.BoxType;
        registration.BoxSize = boxRegistration.BoxSize.GetBoxSize();
        registration.SpecialFeatures = boxRegistration.SpecialFeatures;

        _catBoxDb.SaveChanges();
        
        return registration.Id;
    }

    public async Task<IEnumerable<BoxRegistrationListItemDTO>> GetBoxRegistrationListAsync(bool? isApproved)
    {
        var registrations = await _catBoxDb.BoxRegistrations
            .Include(r => r.Cat)
            .Where(r => isApproved == null || r.IsApproved == isApproved)
            .ToListAsync();

        return registrations.Select(r => new BoxRegistrationListItemDTO()
        {
            Id = r.Id,
            CatId = r.Cat.Id,
            BoxType = r.BoxType,
            BoxSize = r.BoxSize.ToString(),
            SpecialFeatures = r.SpecialFeatures,
            IsApproved = r.IsApproved,
        });
    }
}
