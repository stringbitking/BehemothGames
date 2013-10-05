using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behemoth.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string ImageUrl { get; set; }
        [StringLength(255)]
        public string ScriptUrl { get; set; }
        public DateTime UploadDate { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public int Votes { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<GameRoom> Rooms { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Game() 
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Rooms = new HashSet<GameRoom>();
        }
    }
}
