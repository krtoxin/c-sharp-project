using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Repositories.Interfaces
{
    public interface IChatRoomMemberRepository : IBaseRepository<ChatRoomMember>
    {
        Task<IEnumerable<ChatRoomMember>> GetMembersAsync(int roomId);
    }
}
