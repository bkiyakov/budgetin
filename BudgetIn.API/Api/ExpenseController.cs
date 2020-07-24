using System.Net.Mime;
using System.Threading.Tasks;
using BudgetIn.API.Models;
using BudgetIn.Core.Entities;
using BudgetIn.SharedKernel;
using BudgetIn.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BudgetIn.API.Api
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _repository;

        public ExpenseController(IExpenseRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Expense
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var expenses = await _repository.ListAsync();

            return Ok(expenses);
        }

        // POST: api/Expense
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] ExpenseViewModel expenseViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Expense newExpense = new Expense()
            {
                Sum = expenseViewModel.Sum,
                Note = expenseViewModel.Note,
                CategoryId = expenseViewModel.CategoryId,
                Date = expenseViewModel.Date,
                //TODO изменить userId
                UserId = "userid123123"

            };

            if (await _repository.AddAsync(newExpense))
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(500);
            }
        }

        //GET: api/Expense/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Expense expense = await _repository.GetByIdAsync(id);

            if (expense == null) return NotFound();

            return Ok(expense);
        }
    }
}
