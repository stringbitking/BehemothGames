using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behemoth.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(255)]
        public string ImageUrl { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public virtual ICollection<Game> Games { get; set; }

        public Category()
        {
            this.Games = new HashSet<Game>();
        }
    }
}
