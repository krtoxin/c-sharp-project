using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Core.DTOs
{
    public class AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; } = string.Empty;
        public long TokenExpired { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }
}
