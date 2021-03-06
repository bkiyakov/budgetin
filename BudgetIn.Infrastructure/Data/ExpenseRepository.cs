﻿using BudgetIn.Core.Entities;
using BudgetIn.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetIn.Infrastructure.Data
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _dbContext;
        public ExpenseRepository(AppDbContext dbContextt)
        {
            _dbContext = dbContextt;
        }
        public async Task<bool> AddAsync(Expense expense)
        {
            _dbContext.Expenses.Add(expense);
            return (await _dbContext.SaveChangesAsync() != 0);
        }

        public async Task<bool> DeleteAsync(Expense expense)
        {
            _dbContext.Expenses.Remove(expense);
            return (await _dbContext.SaveChangesAsync() != 0);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            Expense expense = await _dbContext.Expenses.FindAsync(id);

            if (expense == null) return false;

            _dbContext.Expenses.Remove(expense);
            return (await _dbContext.SaveChangesAsync() != 0);
        }

        public async Task<Expense> GetByIdAsync(int id, string userId)
        {
            Expense expense = await _dbContext.Expenses.Where(e => e.UserId == userId).Include(e => e.Category).ThenInclude(c => c.Logo).SingleOrDefaultAsync(e => e.Id == id);

            return expense;
        }

        public async Task<List<Expense>> ListAsync(string userId)
        {
            return await _dbContext.Expenses.Where(e => e.UserId == userId).Include(e => e.Category).ThenInclude(c => c.Logo).OrderBy(e => e.Date).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Expense expense)
        {
            var local = _dbContext.Expenses.Local.FirstOrDefault(e => e.Id.Equals(expense.Id));

            if (local != null)
            {
                // detach
                _dbContext.Entry(local).State = EntityState.Detached;
            }
            else
            {
                return false;
            }

            _dbContext.Entry(expense).State = EntityState.Modified;

            return (await _dbContext.SaveChangesAsync() != 0);
        }
    }
}
