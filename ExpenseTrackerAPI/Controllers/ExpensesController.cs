using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Controllers;

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
        var expenses = await _context.Expenses.ToListAsync();

        return Ok(expenses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense(Expense expense)
    {
        expense.Date = DateTime.UtcNow;

        _context.Expenses.Add(expense);

        await _context.SaveChangesAsync();

        return Ok(expense);
    }
}