using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Core.DTOs
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ApiResult Success() => new() { IsSuccess = true };

        public static ApiResult Failure(params string[] errors)
            => new() { IsSuccess = false, Errors = errors.ToList() };
    }
}
