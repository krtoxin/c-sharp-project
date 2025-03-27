using StudyBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IList<string> roles);
    }

}
