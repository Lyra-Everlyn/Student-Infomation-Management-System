using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(SimsDbContext context) : base(context)
        {
        }

        public async Task<bool> DepartmentExistsAsync(string departmentCode)
        {
            if (string.IsNullOrWhiteSpace(departmentCode)) return false;
            return await _dbSet.AnyAsync(d => d.DepartmentCode.ToLower() == departmentCode.ToLower());
        }
    }
}