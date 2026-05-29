using LibrariesManagementSystem.Api.Data;
using LibrariesManagementSystem.Api.DTOs.Library;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrariesManagementSystem.Api.Services;

public class LibraryService(AppDbContext db) : ILibraryService
{
    public async Task<List<LibraryDto>> GetAll()
    {
        return await db.Libraries
            .Select(l => new LibraryDto
            {
                Id = l.Id,
                Name = l.Name,
                Address = l.Address
            })
            .ToListAsync();
    }
}