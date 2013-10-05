using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behemoth.Models
{
    public class ApplicationUser : User
    {
        [StringLength(255)]
        public string Avatar { get; set; }
        public virtual ICollection<Game> FavouriteGames { get; set; }
    }
}