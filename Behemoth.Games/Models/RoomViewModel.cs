using Behemoth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Behemoth.Games.Models
{
    public class RoomViewModel
    {
        public static Expression<Func<GameRoom, RoomViewModel>> FromGameRoom
        {
            get
            {
                return r => new RoomViewModel
                {
                    Id = r.Id,
                    Status = r.Status,
                    Name = r.Name,
                    Password = r.Password,
                    Player1Id = r.Player1Id,
                    Player2Id = r.Player2Id
                };
            }
        }

        public int Id { get; set; }
        public RoomStatus Status { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
    }
}