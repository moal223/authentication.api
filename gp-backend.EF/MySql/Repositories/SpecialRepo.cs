using gp_backend.EF.MySql.Repositories.Interfaces;
using gp_backend.Core.Models;
using gp_backend.EF.MySql.Data;
using Microsoft.EntityFrameworkCore;

namespace gp_backend.EF.MySql.Repositories
{
    public class SpecialRepo : ISpecialRepo
    {
        private readonly MySqlDbContext _context;
        public SpecialRepo(MySqlDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if (result != null)
                _context.Specializations.Remove(result);
        }

        public async Task DeleteAsync(Specialization entity)
        {
            _context.Specializations.Remove(entity);
        }

        public async Task<List<Specialization>> GetAllAsync(string userId)
        {
            return await _context.Specializations.ToListAsync();
        }

        public async Task<Specialization> GetByIdAsync(int id)
        {
            return await _context.Specializations.Include(x => x.Diseases).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Specialization> InsertAsync(Specialization entity)
        {
            var result = await _context.Specializations.AddAsync(entity);
            return result.Entity;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Specialization> UpdateAsync(Specialization entity)
        {
            var result = _context.Specializations.Update(entity);
            return result.Entity;
        }
    }
}
