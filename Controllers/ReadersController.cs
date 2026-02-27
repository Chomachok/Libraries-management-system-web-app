using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;
using LibrariesWebApp.Services;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class ReadersController(ICrudService<Reader, int> service) : CrudController<Reader, int>(service);