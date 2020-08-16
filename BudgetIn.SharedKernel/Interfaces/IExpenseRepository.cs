﻿using BudgetIn.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetIn.SharedKernel.Interfaces
{
    public interface IExpenseRepository
    {
        Task<Expense> GetByIdAsync(int id);
        Task<List<Expense>> ListAsync();
        Task<bool> AddAsync(Expense expense);
        Task<bool> UpdateAsync(Expense expense);
        Task<bool> DeleteAsync(Expense expense);
        Task<bool> DeleteByIdAsync(int id);
    }
}
