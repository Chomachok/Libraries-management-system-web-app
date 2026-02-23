using LibrariesWebApp.Models;
using LibrariesWebApp.Data;
using LibrariesWebApp.Controllers.Base;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class ReadersController(AppDbContext context) : CrudController<Reader, int>(context);