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
    public class LogoRepository : ILogoRepository
    {
        private readonly AppDbContext _dbContext;

        public LogoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddAsync(Logo logo)
        {
            _dbContext.Add(logo);

            return (await _dbContext.SaveChangesAsync() != 0);
        }

        public async Task<bool> DeleteAsync(Logo logo)
        {
            _dbContext.Remove(logo);

            return (await _dbContext.SaveChangesAsync() != 0);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Logo> GetByIdAsync(int id)
        {
            return await _dbContext.Logos.SingleOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Logo>> ListAsync()
        {
            return await _dbContext.Logos.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Logo logo)
        {
            Logo local = _dbContext.Logos.FirstOrDefault(l => l.Id.Equals(logo.Id));

            if (local != null)
            {
                _dbContext.Entry(local).State = EntityState.Detached;
            } else
            {
                return false;
            }

            _dbContext.Entry(logo).State = EntityState.Modified;

            return (await _dbContext.SaveChangesAsync() != 0);
        }
    }
}
