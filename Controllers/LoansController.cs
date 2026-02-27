using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;
using LibrariesWebApp.Services;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class LoansController(ICrudService<Loan, int> service) : CrudController<Loan, int>(service);