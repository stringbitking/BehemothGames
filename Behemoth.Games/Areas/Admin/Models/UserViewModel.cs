using Behemoth.Models;
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
                    IsAdmin = b.Roles.Any(r => r.Role.Name == "Admin")
                };
            }
        }

        public string Id { get; set; }
        public string Username { get; set; }
        
        public bool IsAdmin { get; set; }
    }
}