﻿using System.Web;
using System.Web.Mvc;

namespace Behemoth.Games
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
