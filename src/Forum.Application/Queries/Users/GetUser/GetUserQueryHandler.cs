using FluentResults;
using Forum.Domain.DTOs;
using Forum.Domain.Interfaces;
using Mediator;

namespace Forum.Application.Queries.Users.GetUser;

public class GetUserQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    public async ValueTask<Result<UserDto>> Handle(GetUserQuery request, CancellationToken ct)
    {
        UserDto? user = await userRepository.GetByIdAsync(request.Id, ct);

        return user is null
            ? Result.Fail($"User with ID {request.Id} not found")
            : Result.Ok(user);
    }
}
