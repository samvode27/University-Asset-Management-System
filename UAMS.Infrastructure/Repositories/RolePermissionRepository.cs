using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Roles;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class RolePermissionRepository
    : GenericRepository<RolePermission>,
      IRolePermissionRepository
{
    public RolePermissionRepository(UAMSDbContext context)
        : base(context)
    {
    }
}