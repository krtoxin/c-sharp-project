﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Services.IServices
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
