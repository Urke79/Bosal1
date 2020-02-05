using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;

namespace BosalMontaze
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));


            // Enable Cross origin
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "DatePerMonthApi",
            routeTemplate: "api/{controller}/events/{month}/{year}",
            defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
            name: "DateApi",
            routeTemplate: "api/{controller}/events/{date}",
            defaults: new { id = RouteParameter.Optional });


            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional });



        }
    }
}
