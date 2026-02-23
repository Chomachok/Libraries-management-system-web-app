using LibrariesWebApp.Models;
using LibrariesWebApp.Data;
using LibrariesWebApp.Controllers.Base;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class BooksController(AppDbContext context) : CrudController<Book, int>(context);