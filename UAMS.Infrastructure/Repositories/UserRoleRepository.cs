using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Users;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class UserRoleRepository
    : GenericRepository<UserRole>,
      IUserRoleRepository
{
    public UserRoleRepository(UAMSDbContext context)
        : base(context)
    {
    }
}