using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Behemoth.Games.Areas.Admin.Models;
using Behemoth.Models;
using System.IO;

namespace Behemoth.Games.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesAdminController : BaseController
    {
        //
        // GET: /Admin/Categories/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReadCategories([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.Data.Categories.All().Select(CategoryViewModel.FromCategory);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateCategory([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            if (category != null && ModelState.IsValid)
            {
                var newCat = new Category
                {
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl
                };

                this.Data.Categories.Add(newCat);
                this.Data.SaveChanges();

                category.Id = newCat.Id;
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCategory([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            var existingCat = this.Data.Categories.All().FirstOrDefault(x => x.Id == category.Id);

            if (category != null && ModelState.IsValid)
            {
                existingCat.Name = category.Name;
                existingCat.Description = category.Description;
                existingCat.ImageUrl = category.ImageUrl;
                this.Data.SaveChanges();
            }

            return Json((new[] { category }.ToDataSourceResult(request, ModelState)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCategory([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            var existingCat = this.Data.Categories.All().FirstOrDefault(x => x.Id == category.Id);

            this.Data.Categories.Delete(existingCat);
            this.Data.SaveChanges();

            return Json(new[] { category }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadFile(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                string extension = Path.GetExtension(upload.FileName);
                string upoadFolder = string.Empty;

                if (extension == ".jpg" || extension == ".png")
                {
                    upoadFolder = "~/img";
                }
                else
                {
                    return new EmptyResult();
                }

                var fileName = Path.GetFileName(upload.FileName);
                var physicalPath = Path.Combine(Server.MapPath(upoadFolder), fileName);

                upload.SaveAs(physicalPath);

            }

            return new HttpStatusCodeResult(200, "File Uploaded");
        }
	}
}