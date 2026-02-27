using LibrariesWebApp.Services;
using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class PublishersController(ICrudService<Publisher, int> service) : CrudController<Publisher, int>(service);