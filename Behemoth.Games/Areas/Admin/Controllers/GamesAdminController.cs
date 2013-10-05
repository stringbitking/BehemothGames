using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Behemoth.Models;
using Behemoth.Data;
using Behemoth.Games.Areas.Admin.Models;
using System.IO;

namespace Behemoth.Games.Areas.Admin.Controllers
{
    public class GamesAdminController : BaseController
    {
        // GET: /Admin/GamesAdmin/
        public ActionResult Index()
        {
            return View(this.Data.Games.All().Select(GameViewModel.FromGame).ToList());
        }

        // GET: /Admin/GamesAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = this.Data.Games.GetById(id.Value);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: /Admin/GamesAdmin/Create
        public ActionResult Create()
        {
            ViewBag.Categories = this.Data.Categories.All().ToList().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View();
        }

        // POST: /Admin/GamesAdmin/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // 
        // Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GameViewModel game)
        {
            if (ModelState.IsValid)
            {
                var category = this.Data.Categories.GetById(game.CategoryId);

                var gameEntity = new Game()
                {
                    Name = game.Name,
                    ImageUrl = game.ImageUrl,
                    ScriptUrl = game.ScriptUrl,
                    UploadDate = DateTime.Now,
                    Description = game.Description,
                };

                gameEntity.Category = category;

                this.Data.Games.Add(gameEntity);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(game);
        }

        // GET: /Admin/GamesAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var game = this.Data.Games.GetById(id.Value);

            if (game == null)
            {
                return HttpNotFound();
            }

            var gameModel = new GameViewModel()
            {
                Name = game.Name,
                ImageUrl = game.ImageUrl,
                ScriptUrl = game.ScriptUrl,
                UploadDate = DateTime.Now,
                Description = game.Description,
                CategoryId = game.Category.Id
            };

            ViewBag.Categories = this.Data.Categories.All().ToList().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(gameModel);
        }

        // POST: /Admin/GamesAdmin/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // 
        // Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GameViewModel game)
        {
            if (ModelState.IsValid)
            {
                var category = this.Data.Categories.GetById(game.CategoryId);
                var gameEntity = this.Data.Games.GetById(game.Id);

                gameEntity.Name = game.Name;
                gameEntity.ImageUrl = game.ImageUrl;
                gameEntity.ScriptUrl = game.ScriptUrl;
                gameEntity.UploadDate = DateTime.Now;
                gameEntity.Description = game.Description;
                gameEntity.Category = category;

                this.Data.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: /Admin/GamesAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = this.Data.Games.GetById(id.Value);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: /Admin/GamesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = this.Data.Games.GetById(id);
            this.Data.Games.Delete(game);
            this.Data.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UploadedImages(HttpPostedFileBase imageUpload)
        {
            if (imageUpload != null)
            {
                string extension = Path.GetExtension(imageUpload.FileName);
                string upoadFolder = string.Empty;

                if (extension == ".jpg" || extension == ".png")
                {
                    upoadFolder = "~/img";
                }
                else
                {
                    return new EmptyResult();
                }

                var fileName = Path.GetFileName(imageUpload.FileName);
                var physicalPath = Path.Combine(Server.MapPath(upoadFolder), fileName);

                imageUpload.SaveAs(physicalPath);

            }

            return Content("");
        }

        public ActionResult UploadedScripts(HttpPostedFileBase scriptUpload)
        {
            if (scriptUpload != null)
            {
                string extension = Path.GetExtension(scriptUpload.FileName);
                string upoadFolder = string.Empty;
                if (extension == ".js")
                {
                    upoadFolder = "~/Scripts/Games";
                }
                else
                {
                    return new EmptyResult();
                }

                var fileName = Path.GetFileName(scriptUpload.FileName);
                var physicalPath = Path.Combine(Server.MapPath(upoadFolder), fileName);

                scriptUpload.SaveAs(physicalPath);

            }

            return Content("");
        }

        protected override void Dispose(bool disposing)
        {
            this.Data.Dispose();
        }
    }
}
