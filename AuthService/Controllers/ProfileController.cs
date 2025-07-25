using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]

public class ProfileController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return NotFound();

        return Ok(new
        {
            user.Email,
            user.FirstName,
            user.LastName,
            //user.AvatarUrl,
            user.DateOfBirth
        });
    }

[Authorize]
[HttpPut("profile")]
public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var user = await _userManager.FindByIdAsync(userId);

    if (user == null) return NotFound();

    user.FirstName = model.FirstName;
    user.LastName = model.LastName;
    user.DateOfBirth = model.DateOfBirth;
    //user.AvatarUrl = model.AvatarUrl;

    await _userManager.UpdateAsync(user);

    return NoContent();
}

}