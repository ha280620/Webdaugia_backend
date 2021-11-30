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
              name: "List Lot By Start",
              url: "phien-dang-dien-ra",
              defaults: new { controller = "Lot", action = "ListLotByDate1", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "List Lot By Register",
               url: "phien-sap-dien-ra",
               defaults: new { controller = "Lot", action = "ListLotByDate2", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "List Ended Lot",
                url: "phien-da-ket-thuc",
                defaults: new { controller = "Lot", action = "ListEndingLot", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Lot Details",
               url: "phien-dau/{SiteTile}-{LotId}",
               defaults: new { controller = "Lot", action = "IncomingLot", LotId = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Ongoing Lot",
               url: "phien-dang-dien-ra/{SiteTile}-{LotId}",
               defaults: new { controller = "Lot", action = "OnGoingLot", LotId = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "List Lot By Category",
               url: "loai-phien-dau/{SiteTile}-{CateId}",
               defaults: new { controller = "Lot", action = "ListLotByCate", CateId = UrlParameter.Optional }
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
             name: "ProfileCustomer",
             url: "thong-tin-tai-khoan",
             defaults: new { controller = "Login", action = "ProfileCustomer", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "ChangePassCustomer",
             url: "thay-doi-mat-khau",
             defaults: new { controller = "Login", action = "ChangePassCustomer", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "AccountManagement",
             url: "quan-ly-tai-khoan",
             defaults: new { controller = "Login", action = "AccountManagement", id = UrlParameter.Optional }
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
