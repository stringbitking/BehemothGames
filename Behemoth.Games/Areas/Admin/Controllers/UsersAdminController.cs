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

namespace Behemoth.Games.Areas.Admin.Controllers
{
    public class UsersAdminController : BaseController
    {
        //
        // GET: /Admin/UsersAdmin/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReadUsers([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.Data.Users.All().Select(UserViewModel.FromUser);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
	}
}