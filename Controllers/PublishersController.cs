using LibrariesWebApp.Data;
using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class PublishersController(AppDbContext context) : CrudController<Publisher, int>(context);