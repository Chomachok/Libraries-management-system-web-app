using LibrariesManagementSystem.Api.DTOs.Library;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

public interface ILibraryService
{
    Task<List<LibraryDto>> GetAll();
}