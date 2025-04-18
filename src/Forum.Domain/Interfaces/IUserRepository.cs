using Forum.Domain.DTOs;

namespace Forum.Domain.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
