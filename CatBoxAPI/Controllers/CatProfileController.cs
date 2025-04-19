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
    public async Task<IActionResult> Create(CatProfileCreationDTO profile, IWebHostEnvironment webHostEnvironment)
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

        if (!Enum.TryParse(profile.PurrferedBoxSize, true, out BoxSize _))
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

        if (!Enum.TryParse(profile.PurrferedBoxSize, true, out BoxSize _))
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CatProfileListItemDTO>>> List([FromQuery] string? filter, [FromQuery] string? sort, IWebHostEnvironment webHostEnvironment)
    {
        BoxSize? filterBySize = null;

        if (!string.IsNullOrEmpty(filter?.Trim()))
        {
            // Accept any non-letter delimiter of field and value
            // (Ex: BoxSize:Small or boxsize+large or boxSize%20=%20extraLarge)
            var filterField = new string([.. filter.Trim().ToCharArray().TakeWhile(char.IsAsciiLetter)]);
            var filterValue = new string([.. filter.Trim().ToCharArray().Reverse().TakeWhile(char.IsAsciiLetter).Reverse()]); //TODO: Consider a more efficient way to get this

            if (filterField.Trim().Equals("boxsize", StringComparison.CurrentCultureIgnoreCase))
            {
                if (Enum.TryParse(filterValue, true, out BoxSize boxSize))
                    filterBySize = boxSize;
                else
                    return BadRequest($"Filter by BoxSize requires one of these values: {string.Join(", ", Enum.GetValues<BoxSize>())}");
            }
            else
                return BadRequest("Filter feature only supports BoxSize");
        }


        // Alternative approach is used here to get the field name and value, just to keep things interesting
        // TODO: Consider using a consistent and extensible approach instead
        bool? sortByAge = null;
        if (!string.IsNullOrEmpty(sort?.Trim()))
        {
            if (!sort.Trim().StartsWith("age", StringComparison.CurrentCultureIgnoreCase))
                return BadRequest($"Sorting feature is provided only for age.");

            // TODO: Consider using a different data type to represent sorting preference, since false may imply unsorted rather than sorted in descending order
            if (sort.Trim().EndsWith("desc", StringComparison.CurrentCultureIgnoreCase))
                sortByAge = false;
            else
                sortByAge = true;
        }


        IEnumerable<CatProfileListItemDTO> list;
        try
        {
            list = await _catProfileService.GetCatProfileListAsync(filterBySize, sortByAge);
        }
        catch (Exception ex)
        {
            string errorMessage = webHostEnvironment.IsDevelopment() ? $"\r\n{ex.Message}\r\n{ex.InnerException}" : "";
            return Problem($"Request for cat profile list encountered an error.{errorMessage}");
        }
        return Ok(list);
    }
}