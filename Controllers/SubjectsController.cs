using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;
using LibrariesWebApp.Services;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class SubjectsController(ICrudService<Subject, int> service) : CrudController<Subject, int>(service);