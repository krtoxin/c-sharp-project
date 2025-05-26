using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Services.IServices;

namespace StudyBuddyWebBlazor.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class RemindersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public RemindersController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> PostReminder([FromBody] Reminder model)
        {
            var email = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("❌ No identity found in token.");
                return Unauthorized("User identity missing.");
            }

            var user = await _userService.FindByEmailAsync(email);
            if (user == null)
            {
                Console.WriteLine($"❌ No user found for email: {email}");
                return Unauthorized("User not found.");
            }

            model.UserId = user.Id;

            _context.Reminders.Add(model);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Saved Reminder for {user.Email}");
            return Ok(model);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingReminders()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("❌ No identity in JWT.");
                return Unauthorized("User identity missing.");
            }

            var user = await _userService.FindByEmailAsync(email);
            if (user == null)
            {
                Console.WriteLine($"❌ User not found: {email}");
                return Unauthorized("User not found.");
            }

            var reminders = await _context.Reminders
                .Where(r => r.UserId == user.Id && !r.IsSent && r.RemindAt <= DateTime.UtcNow)
                .OrderBy(r => r.RemindAt)
                .ToListAsync();

            return Ok(reminders);
        }
    }
}
