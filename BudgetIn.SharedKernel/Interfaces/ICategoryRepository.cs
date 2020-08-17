using BudgetIn.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetIn.SharedKernel.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id, string userId);
        Task<List<Category>> ListAsync(string userId);
        Task<bool> AddAsync(Category category);
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteAsync(Category category);
        Task<bool> DeleteByIdAsync(int id);
    }
}
