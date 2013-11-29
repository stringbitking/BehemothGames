using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Behemoth.Games.Areas.Admin.Models;
using System.Web.Security;
using Behemoth.Data;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Behemoth.Games.Areas.Admin.Controllers
{
    public class UsersAdminController : BaseController
    {
        //
        // GET: /Admin/UsersAdmin/
        public ActionResult Index()
        {
            var allRoles = this.Data.Roles.All();

            ViewBag.Roles = (from role in allRoles
                              select new SelectListItem()
                              {
                                  Text = role.Name,
                                  Value = role.Name
                              });
            return View();
        }

        public JsonResult ReadUsers([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.Data.Users.All().Select(UserViewModel.FromUser);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateUser([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {
            var existingUser = this.Data.Users.All().FirstOrDefault(x => x.Id == user.Id);

            if (user != null && ModelState.IsValid)
            {
                var newRole = this.Data.Roles.All().FirstOrDefault(x => x.Name == user.Role);

                if (newRole != null)
                {
                    var userId = existingUser.Id;
                    UserRole oldRole = this.Data.UserRoles.All().
                        FirstOrDefault(r => r.UserId == userId);
                    if (oldRole != null)
                    {
                        this.Data.UserRoles.Delete(oldRole);
                    }

                    UserRole newUserRole = new UserRole();
                    newUserRole.User = existingUser;
                    newUserRole.Role = newRole;
                    existingUser.Roles.Add(newUserRole);

                    this.Data.SaveChanges();
                }
            }

            return Json((new[] { user }.ToDataSourceResult(request, ModelState)), JsonRequestBehavior.AllowGet);
        }
    }
}