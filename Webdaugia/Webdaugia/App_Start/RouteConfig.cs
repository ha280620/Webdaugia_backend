using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Webdaugia
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Login", action = "DangNhap" }
            );
            routes.MapRoute(
                name: "DangKi",
                url: "dang-ki",
                defaults: new { controller = "Login", action = "DangKi" }
            );
            routes.MapRoute(
                 name: "Quenmatkhau",
                 url: "quen-mat-khau",
                 defaults: new { controller = "Login", action = "Quenmatkhau" }
                );
            routes.MapRoute(
             name: "Themthongtin",
             url: "them-thong-tin",
             defaults: new { controller = "Login", action = "Themthongtin" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
