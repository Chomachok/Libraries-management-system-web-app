namespace LibrariesManagementSystem.Api.Models;

public class Library
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
    public ICollection<User> Users { get; set; } = new List<User>();
}