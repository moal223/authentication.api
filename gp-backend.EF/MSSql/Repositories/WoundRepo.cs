using gp_backend.EF.MSSql.Repositories.Interfaces;
using gp_backend.Core.Models;
using gp_backend.EF.MSSql.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gp_backend.EF.MSSql.Repositories
{
    public class WoundRepo : IGenericRepo<Wound>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public WoundRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Wounds.Remove(await GetByIdAsync(id));
        }
        public async Task DeleteAsync(Wound entity)
        {
            _context.Wounds.Remove(entity);
        }
        public async Task<List<Wound>> GetAllAsync(string userId)
        {
            return _context.Wounds.Include(x => x.Disease).Include(x => x.Image).Where(x => x.ApplicationUserId == userId).ToList();
        }

        public async Task<Wound> GetByIdAsync(int id)
        {
            return await _context.Wounds.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Wound> InsertAsync(Wound entity)
        {
            var result = await _context.Wounds.AddAsync(entity);
            return result.Entity;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Wound> UpdateAsync(Wound entity)
        {
            var result = _context.Wounds.Update(entity);
            return result.Entity;
        }
    }
}
