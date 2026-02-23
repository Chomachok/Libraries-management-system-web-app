using LibrariesWebApp.Data;
using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class SubjectsController(AppDbContext context) : CrudController<Subject, int>(context);