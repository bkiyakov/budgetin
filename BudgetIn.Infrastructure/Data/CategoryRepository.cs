using BudgetIn.Core.Entities;
using BudgetIn.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetIn.Infrastructure.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddAsync(Category category)
        {
            _dbContext.Categories.Add(category);

            return (await _dbContext.SaveChangesAsync() != 0);
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            _dbContext.Categories.Remove(category);

            return (await _dbContext.SaveChangesAsync() != 0);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            Category category = await _dbContext.Categories.FindAsync(id);

            if (category == null) return false;

            _dbContext.Categories.Remove(category);

            return (await _dbContext.SaveChangesAsync() != 0);
        }

        public async Task<Category> GetByIdAsync(int id, string userId)
        {
            Category category = await _dbContext.Categories.Where(c => c.UserId == userId || c.UserId == "system_user").Include(c => c.Logo).SingleOrDefaultAsync(c => c.Id.Equals(id));

            return category;
        }

        public async Task<List<Category>> ListAsync(string userId)
        {
            return await _dbContext.Categories.Where(c => c.UserId == userId || c.UserId == "system_user").Include(c => c.Logo).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var local = _dbContext.Categories.Local.FirstOrDefault(c => c.Id.Equals(category.Id));

            if (local != null)
            {
                // detach
                _dbContext.Entry(local).State = EntityState.Detached;
            }
            else
            {
                return false;
            }

            _dbContext.Entry(category).State = EntityState.Modified;

            return (await _dbContext.SaveChangesAsync() != 0);
        }
    }
}
