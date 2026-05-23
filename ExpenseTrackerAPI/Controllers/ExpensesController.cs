using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExpensesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        var userId = int.Parse(User.FindFirst("sub")!.Value);

        var expenses = await _context.Expenses
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return Ok(expenses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense(Expense expense)
    {
        var userId = int.Parse(User.FindFirst("sub")!.Value);

        expense.UserId = userId;
        expense.Date = DateTime.UtcNow;

        _context.Expenses.Add(expense);

        await _context.SaveChangesAsync();

        return Ok(expense);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, Expense updatedExpense)
    {
        var userId = int.Parse(User.FindFirst("sub")!.Value);

        var expense = await _context.Expenses
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (expense == null)
            return NotFound();

        expense.Title = updatedExpense.Title;
        expense.Amount = updatedExpense.Amount;
        expense.Date = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var userId = int.Parse(User.FindFirst("sub")!.Value);

        var expense = await _context.Expenses
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (expense == null)
            return NotFound();

        _context.Expenses.Remove(expense);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}