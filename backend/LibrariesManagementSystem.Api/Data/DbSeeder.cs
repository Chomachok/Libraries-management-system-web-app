using LibrariesManagementSystem.Api.Models;

namespace LibrariesManagementSystem.Api.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Libraries.Any())
        {
            var lib = new Library
            {
                Name = "Главная библиотека",
                Address = "ул. Книжная, 1"
            };
            context.Libraries.Add(lib);
            context.SaveChanges();

            var librarian = new User
            {
                FullName = "Библиотекарь Иванов",
                Email = "librarian@lib.ru",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Librarian1!"),
                Role = UserRole.Librarian,
                LibraryId = lib.Id
            };
            context.Users.Add(librarian);

            var reader = new User
            {
                FullName = "Читатель Петров",
                Email = "reader@lib.ru",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Reader1!"),
                Role = UserRole.Reader,
                LibraryId = lib.Id
            };
            context.Users.Add(reader);

            // Добавим несколько книг для демонстрации
            var books = new List<Book>
            {
                new Book { Title = "Война и мир", Author = "Лев Толстой", ISBN = "978-5-17-091111-1", Genre = "Роман", Year = 1869, TotalCopies = 5, AvailableCopies = 5, LibraryId = lib.Id },
                new Book { Title = "Преступление и наказание", Author = "Фёдор Достоевский", ISBN = "978-5-17-091112-8", Genre = "Роман", Year = 1866, TotalCopies = 3, AvailableCopies = 3, LibraryId = lib.Id },
                new Book { Title = "1984", Author = "Джордж Оруэлл", ISBN = "978-5-17-091113-5", Genre = "Антиутопия", Year = 1949, TotalCopies = 4, AvailableCopies = 4, LibraryId = lib.Id }
            };
            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}