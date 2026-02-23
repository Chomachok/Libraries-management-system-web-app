using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;
using LibrariesWebApp.Data;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class LibrariesController(AppDbContext context) : CrudController<Library, int>(context);