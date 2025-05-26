using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using StudyBuddy.Core.Entities;

namespace StudyBuddyWebBlazor.Services
{
    public class ReminderService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public ReminderService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        private async Task<bool> AttachAuthHeaderAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("accessToken"); // ✅ correct key!
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("❌ No access token found in local storage.");
                return false;
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine("✅ Attached token to Authorization header.");
            return true;
        }

        public async Task CreateReminder(Reminder reminder)
        {
            Console.WriteLine("📤 Sending Reminder:");
            Console.WriteLine($"Message: {reminder.CustomMessage}");
            Console.WriteLine($"RemindAt: {reminder.RemindAt}");
            Console.WriteLine($"Notify: {reminder.NotifyMinutesBefore}");

            if (!await AttachAuthHeaderAsync())
            {
                Console.WriteLine("❌ Not authenticated.");
                return;
            }

            var response = await _http.PostAsJsonAsync("api/reminders", reminder);
            Console.WriteLine($"📥 Response: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ API Error: {error}");
            }
        }

        public async Task<List<Reminder>> GetUpcomingReminders()
        {
            if (!await AttachAuthHeaderAsync())
            {
                Console.WriteLine("❌ Not authenticated.");
                return new List<Reminder>();
            }

            return await _http.GetFromJsonAsync<List<Reminder>>("api/reminders/upcoming") ?? new List<Reminder>();
        }
    }
}
