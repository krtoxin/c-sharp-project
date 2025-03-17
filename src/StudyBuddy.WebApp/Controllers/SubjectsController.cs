using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;

[Authorize(Roles = "Admin")]
public class SubjectsController : Controller
{
    private readonly AppDbContext _context;

    public SubjectsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Subjects.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Subject subject)
    {
        if (ModelState.IsValid)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(subject);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();
        return View(subject);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Subject subject)
    {
        if (ModelState.IsValid)
        {
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(subject);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
