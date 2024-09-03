using gp_backend.EF.MySql.Repositories.Interfaces;
using gp_backend.Core.Models;
using gp_backend.EF.MySql.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gp_backend.EF.MySql.Repositories
{
    public class FeedBackRepo : IFeedBackRepo
    {
        private readonly MySqlDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public FeedBackRepo(MySqlDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task DeleteAsync(int id)
        {
            _context.FeedBacks.Remove(await GetByIdAsync(id));
        }

        public async Task DeleteAsync(FeedBack entity)
        {
            _context.FeedBacks.Remove(entity);
        }

        public async Task<List<FeedBack>> GetAllAsync()
        {
            return await _context.FeedBacks.ToListAsync();
        }

        public async Task<List<FeedBack>> GetAllAsync(string userId)
        {
            return await _context.FeedBacks.Where(x => x.ApplicationUserId == userId).ToListAsync();
        }

        public async Task<FeedBack> GetByIdAsync(int id)
        {
            return await _context.FeedBacks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<FeedBack> InsertAsync(FeedBack entity)
        {
            var result = await _context.FeedBacks.AddAsync(entity);
            return result.Entity;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<FeedBack> UpdateAsync(FeedBack entity)
        {
            var result = _context.FeedBacks.Update(entity);
            return result.Entity;
        }
    }
}
