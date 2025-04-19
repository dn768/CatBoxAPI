using CatBoxAPI.Enums;
using CatBoxAPI.Models.CatProfile;
using CatBoxAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatBoxAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CatProfileController(ICatProfileService catProfileService) : ControllerBase
{
    private readonly ICatProfileService _catProfileService = catProfileService;

    [HttpPost]
    public async Task<IActionResult> Register(CatProfileCreationDTO profile, IWebHostEnvironment webHostEnvironment)
    {
        if (profile.Name.Trim() == "")
            return BadRequest("Name is required.");

        if (profile.Age < 0)
            return BadRequest("Age should not be negative.");

        if (profile.Age > 40)
            return BadRequest("Cats should not be older that 40 years old.");

        if (profile.Color.Trim() == "")
            return BadRequest("Color is required.");

        if (profile.Weight < 0)
            return BadRequest("Weight should not be negative");

        if (profile.Weight > 65)
            return BadRequest("Cats should not weigh more than 65 lbs.");

        if (profile.Sex.Trim() == "")
            return BadRequest("Sex is required.");

        if (!Enum.TryParse(profile.PurrferedBoxSize, out BoxSize _))
            return BadRequest($"PurrferedBoxSize should be one of these values: {string.Join(", ", Enum.GetValues<BoxSize>())}");

        Guid id;
        try
        {
            id = await _catProfileService.CreateCatProfileAsync(profile);
        }
        catch (Exception ex)
        {
            string errorMessage = webHostEnvironment.IsDevelopment() ? $"\r\n{ex.Message}\r\n{ex.InnerException}" : "";
            
            return Problem($"Cat profile registration encountered an error.{errorMessage}");
        }

        return Ok(id);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(CatProfileEditDTO profile, IWebHostEnvironment webHostEnvironment)
    {
        if (profile.Id == Guid.Empty)
            return BadRequest("Id is required.");

        if (profile.Weight < 0)
            return BadRequest("Weight should not be negative");

        if (profile.Weight > 65)
            return BadRequest("Cats should not weigh more than 65 lbs.");

        if (!Enum.TryParse(profile.PurrferedBoxSize, out BoxSize _))
            return BadRequest($"PurrferedBoxSize should be one of these values: {string.Join(", ", Enum.GetValues<BoxSize>())}");

        try
        {
            await _catProfileService.UpdateCatProfileAsync(profile);
        }
        catch (Exception ex)
        {
            if (ex is UserFriendlyException fe)
                return NotFound(fe.Message);

            string errorMessage = webHostEnvironment.IsDevelopment() ? $"\r\n{ex.Message}\r\n{ex.InnerException}" : "";

            return Problem($"Cat profile update encountered an error.{errorMessage}");
        }

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, IWebHostEnvironment webHostEnvironment)
    {
        if (id == Guid.Empty)
            return BadRequest("Id is required.");

        try
        {
            await _catProfileService.DeleteCatProfileAsync(id);
        }
        catch (Exception ex)
        {
            if (ex is UserFriendlyException fe)
                return NotFound(fe.Message);

            string errorMessage = webHostEnvironment.IsDevelopment() ? $"\r\n{ex.Message}\r\n{ex.InnerException}" : "";
            return Problem($"Cat profile deletion encountered an error.{errorMessage}");
        }
        return Ok();
    }
}