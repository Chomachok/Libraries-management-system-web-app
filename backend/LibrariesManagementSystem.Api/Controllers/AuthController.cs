using LibrariesManagementSystem.Api.DTOs.Auth;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.Register(dto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.Login(dto);
        return Ok(result);
    }
}