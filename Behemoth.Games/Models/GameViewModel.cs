using Behemoth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Behemoth.Games.Models 
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
                    
                };
            }
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [StringLength(255)]
        public string ScriptUrl { get; set; }
        public DateTime UploadDate { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public bool IsFavourite { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Votes { get; set; }
    }
}