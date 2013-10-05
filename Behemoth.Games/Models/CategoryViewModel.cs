using Behemoth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Behemoth.Games.Models
{
    public class CategoryViewModel
    {
        public static Expression<Func<Category, CategoryViewModel>> FromCategory
        {
            get
            {
                return b => new CategoryViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    ImageUrl = b.ImageUrl,
                    Description = b.Description,
                    Games = (from game in b.Games
                             select new GameViewModel()
                             {
                                 Id = game.Id,
                                 Name = game.Name,
                                 ImageUrl = game.ImageUrl,
                                 ScriptUrl = game.ScriptUrl,
                                 Description = game.Description,
                                 UploadDate = game.UploadDate
                             }).ToList()
                };
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        
        public virtual ICollection<GameViewModel> Games { get; set; }
    }
}