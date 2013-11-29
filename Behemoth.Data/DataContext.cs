using Behemoth.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behemoth.Data
{
    public class DataContext : IdentityDbContextWithCustomUser<ApplicationUser>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<Score> Scores { get; set; }

        public DbSet<Behemoth.Models.Category> Categories { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
