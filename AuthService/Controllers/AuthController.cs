using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuthService.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AuthController> _logger;


    public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager, ILogger<AuthController> logger)
    {
        _authService = authService;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost("register")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var (accessToken, refreshToken) = await _authService.LoginAsync(dto);
            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        try
        {
            var (newAccessToken, newRefreshToken) = await _authService.RefreshTokenAsync(dto.RefreshToken);
            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid userId)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserInfoDto>> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var allClaims = User.Claims.Select(c => $"{c.Type}: {c.Value}");
        _logger.LogInformation("CLAIMS: " + string.Join(", ", allClaims));

        if (userId == null)
            return Unauthorized("�� ������� �������� ID ����������� � ������.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound("���������� �� ���������.");

        var userInfo = new UserInfoDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarUrl = user.AvatarUrl,
            DateOfBirth = user.DateOfBirth,
            PhoneNumber = user.PhoneNumber
        };

        return Ok(userInfo);
    }
}


