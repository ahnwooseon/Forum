using System.Text.Json;
using Forum.Domain.DTOs;
using Forum.Domain.Entities;
using Forum.Domain.Interfaces;
using Forum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Forum.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context, IDistributedCache cache) : IUserRepository
{
    private const string CacheKeyPrefix = "users:";

    public async Task<UserDto?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        UserDto? user = await GetFromCacheAsync(id, ct);
        if (user is not null)
        {
            return user;
        }

        user = await GetFromDbAsync(id, ct);
        if (user is not null)
        {
            await SaveToCacheAsync(user);
        }

        return user;
    }

    private async Task<UserDto?> GetFromCacheAsync(string id, CancellationToken ct = default)
    {
        string cacheKey = $"{CacheKeyPrefix}{id}";
        string? cachedUser = await cache.GetStringAsync(cacheKey, ct);

        return cachedUser is null ? null : JsonSerializer.Deserialize<UserDto>(cachedUser);
    }

    private async Task<UserDto?> GetFromDbAsync(string id, CancellationToken ct = default)
    {
        User? user = await context.Users.FindAsync([id], cancellationToken: ct);
        return user is null ? null : new UserDto(user.Id, user.UserName);
    }

    private async Task SaveToCacheAsync(UserDto user, CancellationToken ct = default)
    {
        string cacheKey = $"{CacheKeyPrefix}{user.Id}";
        string cachedUser = JsonSerializer.Serialize(user);
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
        };
        await cache.SetStringAsync(cacheKey, cachedUser, options, ct);
    }
}
