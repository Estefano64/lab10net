using Microsoft.EntityFrameworkCore;
using lab10.Domain.Entities;
using lab10.Domain.Interfaces;
using lab10.Infrastructure.Data;

namespace lab10.Infrastructure.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(TicketeraDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string roleName)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.RoleName == roleName);
    }
}
