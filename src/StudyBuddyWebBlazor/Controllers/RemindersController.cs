using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Core.DTOs;
using StudyBuddy.Services.IServices;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RemindersController : ControllerBase
{
    private readonly IReminderService _reminderService;

    public RemindersController(IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    [HttpPost]
    public async Task<IActionResult> PostReminder([FromBody] ReminderDto dto)
    {
        var userId = User.FindFirst("nameid")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        dto.UserId = userId;

        await _reminderService.CreateReminderAsync(dto);
        return Ok();
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingReminders()
    {
        var userId = User.FindFirst("nameid")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var reminders = await _reminderService.GetUpcomingRemindersAsync(userId);
        return Ok(reminders);
    }
}
