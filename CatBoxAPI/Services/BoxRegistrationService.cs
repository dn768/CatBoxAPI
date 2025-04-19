using CatBoxAPI.DB;
using CatBoxAPI.DB.Entities;
using CatBoxAPI.Enums;
using CatBoxAPI.Extensions;
using CatBoxAPI.Models.BoxRegistration;

namespace CatBoxAPI.Services;

public class BoxRegistrationService(CatBoxContext catBoxDb) : IBoxRegistrationService
{
    private readonly CatBoxContext _catBoxDb = catBoxDb;
    
    public async Task<Guid> CreateAsync(BoxRegistrationCreationDTO boxRegistration)
    {
        var boxRegistrationEntity = new BoxRegistration()
        {
            Id = Guid.NewGuid(),
            Cat = new CatProfileEntity()
            {
                Id = boxRegistration.CatId,
                // TODO: Improve this. This shouldn't end up in the database anyway, but it's ugly and risky.
                Name = "",
                Color = "",
                PurrferedBoxSize = BoxSize.Small,
                Sex = "",
            },
            BoxType = boxRegistration.BoxType,
            BoxSize = boxRegistration.BoxSize.GetBoxSize(),
            SpecialFeatures = boxRegistration.SpecialFeatures,
            IsApproved = false,
            CreatedAt = DateTime.UtcNow,
        };
        
        await _catBoxDb.BoxRegistrations.AddAsync(boxRegistrationEntity);
        await _catBoxDb.SaveChangesAsync();

        return boxRegistrationEntity.Id;
    }
}
