using BudgetIn.Core.Entities;
using BudgetIn.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Logo> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Logo>> ListAsync()
        {
            return await _dbContext.Logos.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Logo logo)
        {
            throw new NotImplementedException();
        }
    }
}
