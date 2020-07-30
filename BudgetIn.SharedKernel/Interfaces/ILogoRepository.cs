using BudgetIn.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetIn.SharedKernel.Interfaces
{
    public interface ILogoRepository
    {
        Task<Logo> GetByIdAsync(int id);
        Task<List<Logo>> ListAsync();
        Task<bool> AddAsync(Logo logo);
        Task<bool> UpdateAsync(Logo logo);
        Task<bool> DeleteAsync(Logo logo);
        Task<bool> DeleteByIdAsync(int id);
    }
}
