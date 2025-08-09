using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _rtRepo;
    public AccountController(UserManager<ApplicationUser> userManager, ITokenService tokenService, IRefreshTokenRepository rtRepo)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _rtRepo = rtRepo;
    }
    // GET /account/login
    [HttpGet("login")]
public IActionResult Login(string returnUrl = "/account/google-response")
{
    if (string.IsNullOrEmpty(returnUrl) || returnUrl.Contains("login"))
        returnUrl = "/account/google-response";

    var props = new AuthenticationProperties
    {
        RedirectUri = returnUrl
    };

    return Challenge(props, GoogleDefaults.AuthenticationScheme);
}


  
    
    
[HttpGet("logout")]
[Authorize] 
public IActionResult Logout()
{
    
    return SignOut(
        new AuthenticationProperties
        {
            RedirectUri = "/" 
        },
        CookieAuthenticationDefaults.AuthenticationScheme
    );
}


[HttpGet("userinfo")]
[Authorize] 
public IActionResult UserInfo()
{
    var user = HttpContext.User;

    if (user.Identity?.IsAuthenticated ?? false)
    {
        return Ok(new
        {
            Name = user.Identity.Name,
            Email = user.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value,
            AuthenticatedWith = user.Identity.AuthenticationType 
        });
    }

    return Unauthorized();
}


    
    [HttpGet("google-response")]
public async Task<IActionResult> GoogleResponse()
{
    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

    if (!result.Succeeded)
    {
        return Unauthorized();
    }

    var claims = result.Principal.Claims;
    var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

    if (email == null)
        return BadRequest("Email claim not found.");

    
    var user = await _userManager.FindByEmailAsync(email);
if (user == null)
{
    user = new ApplicationUser
    {
        UserName = email,
        Email = email,
        EmailConfirmed = true 
    };

    var res = await _userManager.CreateAsync(user);
    if (!res.Succeeded)
    {
        
        return BadRequest(res.Errors);
    }
}


    
    var jwtToken = _tokenService.GenerateAccessToken(user);
    var refreshToken = _tokenService.GenerateRefreshToken(user);

    
    await _rtRepo.AddAsync(refreshToken);
    await _rtRepo.SaveChangesAsync();

    
        return Ok(new
        {
            token = jwtToken,
            refreshToken = refreshToken
        });
}

}
