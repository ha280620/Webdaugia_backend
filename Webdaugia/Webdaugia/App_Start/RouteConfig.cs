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
               name: "List Lot By Category",
               url: "loai-phien-dau/{CateId}",
               defaults: new { controller = "Lot", action = "ListLotByCate", id = UrlParameter.Optional }

           );

            routes.MapRoute(
               name: "Lot Details",
               url: "phien-dau/{LotId}",
               defaults: new { controller = "Lot", action = "IncomingLot", id = UrlParameter.Optional }

           );

            routes.MapRoute(
               name: "Ongoing Lot",
               url: "phien-dang-dien-ra/{LotId}",
               defaults: new { controller = "Lot", action = "OnGoingLot", id = UrlParameter.Optional }

           );

            routes.MapRoute(
               name: "Dang Ky Dau Gia",
               url: "dang-ky-dau-gia",
               defaults: new { controller = "Lot", action = "RegisterBid", id = UrlParameter.Optional }

           );
            
            routes.MapRoute(
               name: "Dat Gia Dau Gia",
               url: "dat-gia-dau-gia",
               defaults: new { controller = "Lot", action = "PlaceBid", id = UrlParameter.Optional }

           );



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
                 url: "Quenmatkhau",
                 defaults: new { controller = "Login", action = "Quenmatkhau" }
                );
           
            //routes.MapRoute(
            //    name: "Login",
            //    url: "dang-nhap",
            //    defaults: new { controller = "Login", action = "DangNhap" }
            //);
            //routes.MapRoute(
            //    name: "DangKi",
            //    url: "dang-ki",
            //    defaults: new { controller = "Login", action = "DangKi" }
            //);
            //routes.MapRoute(
            //     name: "Quenmatkhau",
            //     url: "quen-mat-khau",
            //     defaults: new { controller = "Login", action = "Quenmatkhau" }
            //    );
            //routes.MapRoute(
            // name: "Themthongtin",
            // url: "them-thong-tin",
            // defaults: new { controller = "Login", action = "Themthongtin" }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
           
        }
    }
}
