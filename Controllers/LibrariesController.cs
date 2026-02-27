using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;
using LibrariesWebApp.Services;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class LibrariesController(ICrudService<Library, int> service) : CrudController<Library, int>(service);