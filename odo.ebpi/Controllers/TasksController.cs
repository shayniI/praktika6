using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace odo.ebpi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;

    public TasksController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/tasks?completed=true|false (optional)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks([FromQuery] bool? completed)
    {
        IQueryable<TaskItem> query = _db.Tasks.AsNoTracking();
        if (completed.HasValue)
        {
            query = query.Where(t => t.IsCompleted == completed.Value);
        }
        var items = await query.OrderBy(t => t.IsCompleted).ThenBy(t => t.DueDate).ToListAsync();
        return Ok(items);
    }

    // GET: api/tasks/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        var item = await _db.Tasks.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItem dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var item = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted,
            CreatedAt = DateTime.UtcNow
        };

        _db.Tasks.Add(item);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTask), new { id = item.Id }, item);
    }

    // PUT: api/tasks/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem dto)
    {
        var item = await _db.Tasks.FindAsync(id);
        if (item == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Title)) item.Title = dto.Title;
        item.Description = dto.Description;
        item.IsCompleted = dto.IsCompleted;
        item.DueDate = dto.DueDate;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // PATCH: api/tasks/5/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool isCompleted)
    {
        var item = await _db.Tasks.FindAsync(id);
        if (item == null) return NotFound();
        item.IsCompleted = isCompleted;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/tasks/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var item = await _db.Tasks.FindAsync(id);
        if (item == null) return NotFound();
        _db.Tasks.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}


