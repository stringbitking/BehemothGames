using Behemoth.Games.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Behemoth.Games.Controllers
{
    public class CategoriesController : BaseController
    {
        //
        // GET: /Categories/
        public ActionResult Index()
        {
            var categories = this.Data.Categories.All().Select(CategoryViewModel.FromCategory);
            var user = this.Data.Users.All().FirstOrDefault(usr => usr.UserName == User.Identity.Name);

            foreach (var category in categories)
            {
                foreach (var game in category.Games)
                {
                    if (user != null)
                    {
                        if (user.FavouriteGames.Any(g => g.Id == game.Id))
                        {
                            game.IsFavourite = true;
                        }
                    }
                }
            }

            return View(categories);
        }

        public ActionResult Details(int id)
        {
            var category = this.Data.Categories.GetById(id);
            var user = this.Data.Users.All().FirstOrDefault(usr => usr.UserName == User.Identity.Name);

            var categoryModel = new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl,
                Description = category.Description,
                Games = (from game in category.Games
                         select new GameViewModel()
                         {
                             Id = game.Id,
                             Name = game.Name,
                             ImageUrl = game.ImageUrl,
                             ScriptUrl = game.ScriptUrl,
                             Description = game.Description,
                             UploadDate = game.UploadDate,
                             CategoryName = game.Category.Name
                         }).ToList()
            };

            foreach (var game in categoryModel.Games)
            {
                if (user != null)
                {
                    if (user.FavouriteGames.Any(g => g.Id == game.Id))
                    {
                        game.IsFavourite = true;
                    }
                }
            }

            return View(categoryModel);
        }
	}
}