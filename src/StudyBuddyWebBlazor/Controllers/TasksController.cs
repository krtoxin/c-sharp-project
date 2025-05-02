using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

public class TasksController : Controller
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var tasks = _context.StudyTasks.Include(t => t.SubTopic).ToList();
        return View(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudyTask model)
    {
        _context.StudyTasks.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
