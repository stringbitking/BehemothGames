using Behemoth.Data;
using Behemoth.Games.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Behemoth.Games.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Space()
        {
            return View();
        }

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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}