using Behemoth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Behemoth.Games.Areas.Admin.Models
{
    public class GameViewModel
    {
        public static Expression<Func<Game, GameViewModel>> FromGame
        {
            get
            {
                return b => new GameViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    ImageUrl = b.ImageUrl,
                    ScriptUrl = b.ScriptUrl,
                    UploadDate = b.UploadDate,
                    CategoryName = b.Category.Name
                };
            }
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string ImageUrl { get; set; }
        [StringLength(255)]
        public string ScriptUrl { get; set; }
        [ScaffoldColumn(false)]
        public DateTime UploadDate { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}