using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behemoth.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public virtual Game Game { get; set; }
        public virtual ApplicationUser Player { get; set; }
    }
}
