namespace LibrariesManagementSystem.Api.Models;

public enum UserRole
{
    Reader,
    Librarian
}

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public int LibraryId { get; set; }
    public Library Library { get; set; } = null!;

    public ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();
}