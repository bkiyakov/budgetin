using System.Net.Mime;
using System.Threading.Tasks;
using BudgetIn.Core.Entities;
using BudgetIn.SharedKernel;
using BudgetIn.SharedKernel.Interfaces;
using BudgetIn.WebApi.Identity.Models;
using BudgetIn.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BudgetIn.WebApi.Controllers
{
    /// <summary>
    /// API Endpoints for get, add, modify and delete Expense
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _repository;
        private readonly UserManager<User> _userManager;

        public ExpenseController(IExpenseRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        /// <summary>
        /// Getting list of expenses
        /// </summary>
        /// <returns>Returns list of expenses</returns>
        // GET: api/Expense
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            var expenses = await _repository.ListAsync(userId);

            return Ok(expenses);
        }

        /// <summary>
        /// Adding expense
        /// </summary>
        /// <param name="expenseViewModel"></param>
        /// <returns>Returns 201 if expense added, 400 with description if input model is not valid and 500 if expense didn't add for some reason</returns>
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

            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            Expense newExpense = new Expense()
            {
                Sum = expenseViewModel.Sum,
                Note = expenseViewModel.Note,
                CategoryId = expenseViewModel.CategoryId,
                Date = expenseViewModel.Date,
                UserId = userId

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

        /// <summary>
        /// Getting expense by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns expense if exist, 404 if not</returns>
        //GET: api/Expense/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            Expense expense = await _repository.GetByIdAsync(id, userId);

            if (expense == null) return NotFound();

            return Ok(expense);
        }

        /// <summary>
        /// Modify exists expense
        /// </summary>
        /// <param name="expenseViewModel"></param>
        /// <param name="id">ID of modifying expense</param>
        /// <returns>Returns 200 if expense modified, 404 if expense nof found, 400 with description if input model is not valid</returns>
        // PUT: api/Expense/{id}
        [HttpPut("{id}:int")]
        public async Task<IActionResult> Update([FromBody] ExpenseViewModel expenseViewModel, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            if (await _repository.GetByIdAsync(id, userId) == null) return NotFound();

            Expense newExpense = new Expense()
            {
                Id = id,
                Sum = expenseViewModel.Sum,
                Note = expenseViewModel.Note,
                CategoryId = expenseViewModel.CategoryId,
                Date = expenseViewModel.Date,
                UserId = userId

            };

            if (await _repository.UpdateAsync(newExpense))
            {
                return Ok();
            } else
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Delete expense by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns 200 if expense deleted, 404 if not found, 500 if not deleted</returns>
        // DELETE: api/Expense/{id}
        [HttpDelete("{id}:int")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return StatusCode(401);

            if (await _repository.GetByIdAsync(id, userId) == null) return NotFound();

            if (await _repository.DeleteByIdAsync(id))
            {
                return Ok();
            } else
            {
                return StatusCode(500);
            }
        }
    }
}
