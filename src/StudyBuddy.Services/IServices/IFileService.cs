using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Services.IServices
{
    public interface IFileService
    {
        Task<string> SaveTaskImageAsync(IBrowserFile file);
    }
}
