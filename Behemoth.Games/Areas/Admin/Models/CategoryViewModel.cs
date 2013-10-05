using Behemoth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Behemoth.Games.Areas.Admin.Models
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
                    Description = b.Description,
                    ImageUrl = b.ImageUrl,
                };
            }
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [AllowHtml()]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [AllowHtml()]
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [Required]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }
    }
}