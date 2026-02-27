using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;
using LibrariesWebApp.Services;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class BooksController(ICrudService<Book, int> service) : CrudController<Book, int>(service);