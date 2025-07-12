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
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _rtRepo;
    public AccountController(UserManager<IdentityUser> userManager, ITokenService tokenService, IRefreshTokenRepository rtRepo)
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


  
    
    // GET /account/logout
[HttpGet("logout")]
[Authorize] // дозволяє як JWT, так і cookie
public IActionResult Logout()
{
    // Якщо користувач автентифікований через cookie — вийде
    // Якщо через JWT — цей метод не має сенсу (немає сесії)
    return SignOut(
        new AuthenticationProperties
        {
            RedirectUri = "/" // можеш змінити на інше
        },
        CookieAuthenticationDefaults.AuthenticationScheme
    );
}

// GET /account/userinfo
[HttpGet("userinfo")]
[Authorize] // дозволяє і JWT, і cookie
public IActionResult UserInfo()
{
    var user = HttpContext.User;

    if (user.Identity?.IsAuthenticated ?? false)
    {
        return Ok(new
        {
            Name = user.Identity.Name,
            Email = user.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value,
            AuthenticatedWith = user.Identity.AuthenticationType // "Cookies" або "JwtBearer"
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

    // 1. Знайти або створити користувача у БД
    var user = await _userManager.FindByEmailAsync(email);
if (user == null)
{
    user = new IdentityUser
    {
        UserName = email,
        Email = email,
        EmailConfirmed = true // Якщо хочеш одразу підтверджувати
    };

    var res = await _userManager.CreateAsync(user);
    if (!res.Succeeded)
    {
        // Обробити помилки створення користувача
        return BadRequest(res.Errors);
    }
}


    // 2. Згенерувати JWT і refresh токени
    var jwtToken = _tokenService.GenerateAccessToken(user);
    var refreshToken = _tokenService.GenerateRefreshToken(user);

    // 3. Зберегти refresh токен в БД або кеші (опціонально)
    await _rtRepo.AddAsync(refreshToken);
    await _rtRepo.SaveChangesAsync();

    // 4. Повернути токени клієнту
        return Ok(new
        {
            token = jwtToken,
            refreshToken = refreshToken
        });
}

}
