using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Core.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=ep-broad-block-a2q710fh-pooler.eu-central-1.aws.neon.tech;Database=StudyBuddy;Username=StudyBuddy_owner;Password=npg_bHlvj7SCe1Pc;Ssl Mode=Require");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
