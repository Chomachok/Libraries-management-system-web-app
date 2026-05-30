namespace LibrariesManagementSystem.Api.DTOs.Auth;

public class AuthResultDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}