using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behemoth.Models
{
    public enum RoomStatus
    {
        Empty = 0,
        Waiting = 1,
        Active = 2,
        Finished = 3
    }

    public class GameRoom
    {
        [Key]
        public int Id { get; set; }
        public RoomStatus Status { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(30)]
        public string Password { get; set; }
        public string Player1Id { get; set; }
        [StringLength(255)]
        public string Player2Id { get; set; }
        public virtual Game Game { get; set; }
    }
}
