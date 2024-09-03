using gp_backend.EF.MySql.Repositories.Interfaces;
using gp_backend.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using gp_backend.EF.MySql.Data;

namespace gp_backend.EF.MySql.Repositories
{
    public class DiseaseRepo : IGenericRepo<Disease>
    {
        private readonly MySqlDbContext _context;
        public DiseaseRepo(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if(result != null)
                _context.Diseases.Remove(result);
        }

        public async Task DeleteAsync(Disease entity)
        {
            _context.Diseases.Remove(entity);
        }

        public async Task<List<Disease>> GetAllAsync(string userId)
        {
            return _context.Diseases.ToList();
        }

        public async Task<Disease> GetByIdAsync(int id)
        {
            return await _context.Diseases.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Disease> InsertAsync(Disease entity)
        {
            var result = await _context.Diseases.AddAsync(entity);
            return result.Entity;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Disease> UpdateAsync(Disease entity)
        {
            var result = _context.Diseases.Update(entity);
            return result.Entity;
        }
    }
}
