using LibrariesWebApp.Data;
using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class LoansController(AppDbContext context) : CrudController<Loan, int>(context);