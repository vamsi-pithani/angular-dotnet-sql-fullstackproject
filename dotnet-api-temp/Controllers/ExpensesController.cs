using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetApi.Data;
using DotnetApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpensesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GET_ALL_EXPENSE/{userId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpenses(string userId)
        {
            var expenses = await _context.Expenses
                .AsNoTracking()
                .Where(e => e.Creater == userId)
                .ToListAsync();
            return Ok(expenses);
        }

        [HttpPost("CREATE_EXPENSE")]
        public async Task<ActionResult<Expense>> CreateExpense([FromBody] Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
        }

        [HttpGet("GET_SINGLE_EXPENSE/{userId}/{id}")]
        public async Task<ActionResult<Expense>> GetExpenseById(string userId, int id)
        {
            var expense = await _context.Expenses
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id && e.Creater == userId);

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        [HttpPatch("UPDATE_EXPENSE/{userId}/{id}")]
        public async Task<IActionResult> UpdateExpense(string userId, int id, [FromBody] Expense updatedExpense)
        {
            if (id != updatedExpense.Id || userId != updatedExpense.Creater)
            {
                return BadRequest();
            }

            _context.Entry(updatedExpense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id, userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("DELETE_EXPENSE/{userId}/{id}")]
        public async Task<IActionResult> DeleteExpense(string userId, int id)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.Creater == userId);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseExists(int id, string userId)
        {
            return _context.Expenses.Any(e => e.Id == id && e.Creater == userId);
        }
    }
}
