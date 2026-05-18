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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, Expense expense)
    {
        // validate ID match
        if (id != expense.Id)
            return BadRequest("Expense ID mismatch");

        // fetch existing entity
        var expenseToUpdate = await _context.Expenses.FindAsync(id);
        if (expenseToUpdate == null) return NotFound($"Employee with ID = {id} not found");

        // update and save
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var expenseToDelete = await _context.Expenses.FindAsync(id);

        if (expenseToDelete == null) return NotFound($"Expense with Id = {id} not found");

        _context.Expenses.Remove(expenseToDelete);

        await _context.SaveChangesAsync();

        return NoContent();


    }

}