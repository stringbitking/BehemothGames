using Behemoth.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Security;

namespace Behemoth.Games.Areas.Admin.Models
{
    public class UserViewModel
    {
        public static Expression<Func<ApplicationUser, UserViewModel>> FromUser
        {
            get
            {
                return b => new UserViewModel
                {
                    Id = b.Id,
                    Username = b.UserName,
                    Role = (b.Roles.FirstOrDefault().Role.Name ?? "No Role"),
                    IsAdmin = b.Roles.Any(r => r.Role.Name == "Admin")
                };
            }
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }
    }
}