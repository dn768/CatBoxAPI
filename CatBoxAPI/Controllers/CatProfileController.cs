using CatBoxAPI.Enums;
using CatBoxAPI.Models.CatProfile;
using CatBoxAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatBoxAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CatProfileController(ICatProfileService catProfileService) : ControllerBase
    {
        private readonly ICatProfileService _catProfileService = catProfileService;

        [HttpPut]
        public async Task<IActionResult> Register(CatProfileCreationDTO profile)
        {
            if (profile.Age > 40)
                return BadRequest("Cats should not be older that 40 years old.");

            if (profile.Weight > 65)
                return BadRequest("Cats should not weigh more than 65 lbs.");

            if (!Enum.TryParse(profile.PurrferedBoxSize, out BoxSize _))
                return BadRequest($"PurrferedBoxSize should be one of these values: {string.Join(", ", Enum.GetValues<BoxSize>())}");

            try
            {
                await _catProfileService.CreateCatProfileAsync(profile);
            }
            catch
            {
                return Problem("Cat profile registration encountered an error.");
            }

            return Ok("Cat profile registration succeeded.");
        }
    }
}
