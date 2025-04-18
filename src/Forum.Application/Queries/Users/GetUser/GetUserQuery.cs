using FluentResults;
using Forum.Domain.DTOs;
using Mediator;

namespace Forum.Application.Queries.Users.GetUser;

public record GetUserQuery(string Id) : IRequest<Result<UserDto>>;
