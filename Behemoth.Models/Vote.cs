﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behemoth.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Stars { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Game Game { get; set; }
    }
}
