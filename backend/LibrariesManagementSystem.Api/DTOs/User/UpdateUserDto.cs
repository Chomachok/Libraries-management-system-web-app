using System.ComponentModel.DataAnnotations;

namespace LibrariesManagementSystem.Api.DTOs.User;

public class UpdateUserDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}