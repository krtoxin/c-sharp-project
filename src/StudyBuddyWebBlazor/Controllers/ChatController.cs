using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.WebApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<IActionResult> Index(int roomId)
        {
            var messages = await _chatRepository.GetMessagesForRoomAsync(roomId);
            ViewBag.RoomId = roomId;
            return View(messages);
        }
    }
}
