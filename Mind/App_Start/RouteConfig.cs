using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mind
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
                
            routes.MapRoute(
                name:"Login",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Login"}
            );
            routes.MapRoute(
                name:"Detail",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Detail"}
            );
            routes.MapRoute(
                name:"Read",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Read"}
            );
            routes.MapRoute(
                name:"Shelf",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Shelf"}
            );
            routes.MapRoute(
                name:"PLogin",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Login", action = "PLogin"}
            );
            routes.MapRoute(
                name:"PSignup",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Login", action = "PSignup"}
            );
            routes.MapRoute(
                name:"GetSortedBooks",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Index", action = "GetSortedBooks"}
            );
            routes.MapRoute(
                name:"GetSection",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Read", action = "GetSection"}
            );
            routes.MapRoute(
                name:"GetBook",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Detail", action = "GetBook"}
            );
            routes.MapRoute(
                name:"GetDanmu",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Read", action = "GetDanmu"}
            );
            routes.MapRoute(
                name:"AddDanmu",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Read", action = "AddDanmu"}
            );
            routes.MapRoute(
                name:"GetComment",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Read", action = "GetComment"}
            );
            routes.MapRoute(
                name:"AddComment",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Read", action = "AddComment"}
            );
            routes.MapRoute(
                name:"GetShelfBooks",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Shelf", action = "GetShelfBooks"}
            );
            routes.MapRoute(
                name:"AddToShelf",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Shelf", action = "AddToShelf"}
            );
            
            routes.MapRoute(
                name:"GetSearchBooks",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Index", action = "GetSearchBooks"}
            );
            routes.MapRoute(
                name:"Answer",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Index", action = "Answer"}
            );
            
            routes.MapRoute(
                name:"ImageProcess",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Answer", action = "ImageProcess"}
            );
            
            routes.MapRoute(
                name:"GetNotices",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Notice", action = "GetNotices"}
            );
            
            routes.MapRoute(
                name:"AddNotice",
                url:"{controller}/{action}/{id}",
                defaults: new {controller = "Notice", action = "AddNotice"}
            );
        }
    }
}