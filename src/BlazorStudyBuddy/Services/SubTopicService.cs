using StudyBuddy.Core.Entities;

namespace StudyBuddyWebBlazor.Services
{
    public class SubTopicService
    {
        private readonly HttpClient _http;

        public SubTopicService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<SubTopic>> GetAllSubTopicsAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<SubTopic>>("api/subtopics");
        }

        public async Task<IEnumerable<SubTopic>> GetSubTopicsBySubjectAsync(int subjectId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<SubTopic>>($"api/subtopics/by-subject/{subjectId}");
        }

        public async Task DeleteSubTopicAsync(int id)
        {
            await _http.DeleteAsync($"api/subtopics/{id}");
        }
    }
}
