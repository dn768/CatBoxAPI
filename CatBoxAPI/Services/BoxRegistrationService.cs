using CatBoxAPI.DB;
using CatBoxAPI.DB.Entities;
using CatBoxAPI.Enums;
using CatBoxAPI.Extensions;
using CatBoxAPI.Models.BoxRegistration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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

    public async Task<IEnumerable<BoxRegistrationListItemDTO>> GetBoxRegistrationListAsync(bool filterByApproval, bool? approvalFilter, ListSortDirection? sortByBoxSize)
    {
        var registrationQuery = _catBoxDb.BoxRegistrations
            .Include(r => r.Cat)
            .AsQueryable();

        if (filterByApproval)
            registrationQuery = registrationQuery.Where(r => r.IsApproved == approvalFilter);

        if (sortByBoxSize == ListSortDirection.Ascending)
            registrationQuery = registrationQuery.OrderBy(r => r.BoxSize);

        else if (sortByBoxSize == ListSortDirection.Descending)
            registrationQuery = registrationQuery.OrderByDescending(r => r.BoxSize);

        var registrations = await registrationQuery.ToListAsync();

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

    public async Task SaveRegistrationApproval(Guid registrationId, bool isApproved, string? decisionReason)
    {
        var registration = await _catBoxDb.BoxRegistrations.FindAsync(registrationId);
        
        if (registration == null)
            throw new UserFriendlyException($"Box registration with id {registrationId} not found.");
        
        registration.IsApproved = isApproved;
        registration.DecidedOn = DateTime.UtcNow;
        registration.DecisionReason = decisionReason;
        _catBoxDb.SaveChanges();
    }
}
