using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class AdmissionApplicationRepository : Repository<AdmissionApplication>, IAdmissionApplicationRepository
    {
        public AdmissionApplicationRepository(SimsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AdmissionApplication>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Department)
                .ToListAsync();
        }
    }
}
