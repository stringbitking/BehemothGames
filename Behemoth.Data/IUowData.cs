namespace Behemoth.Data
{
    using Behemoth.Models;
    using System;


    public interface IUowData : IDisposable
    {
        IRepository<Game> Games { get; }

        IRepository<GameRoom> GameRooms { get; }

        IRepository<Score> Scores { get; }

        IRepository<Category> Categories { get; }

        IRepository<ApplicationUser> Users { get; }

        IRepository<Vote> Votes { get; }

        int SaveChanges();
    }
}
