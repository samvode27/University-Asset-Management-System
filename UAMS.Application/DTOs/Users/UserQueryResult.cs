using UAMS.Domain.Entities.Users;

namespace UAMS.Application.DTOs.Users;

public class UserQueryResult
{
    public IReadOnlyList<User> Items { get; init; }
        = Array.Empty<User>();

    public int TotalCount { get; init; }
}