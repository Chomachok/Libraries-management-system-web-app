using LibrariesManagementSystem.Api.DTOs.User;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetReaders(int librarianLibraryId);
    Task<UserDto> GetById(int userId);
    Task<UserDto> CreateReader(int librarianLibraryId, CreateUserDto dto);
    Task<UserDto> Update(int userId, UpdateUserDto dto);
    Task Delete(int librarianLibraryId, int userId);
    Task<UserDto> UpdateOwnProfile(int userId, UpdateUserDto dto);
}