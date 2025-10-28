using lab10.Domain.Entities;

namespace lab10.Domain.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string roleName);
}
