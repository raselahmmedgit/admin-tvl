using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using RnD.TVLSec.Models;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace RnD.TVLSec
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            SetIocContainer();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private static void SetIocContainer()
        {
            try
            {
                //Implement Autofac

                //var configuration = System.Web.Http.GlobalConfiguration.Configuration;
                var builder = new ContainerBuilder();

                // Register MVC controllers using assembly scanning.
                builder.RegisterControllers(Assembly.GetExecutingAssembly());

                // Register MVC controller and API controller dependencies per request.
                builder.Register(c => new AppDbEntities()).InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(Repository<>)).InstancePerLifetimeScope();

                var container = builder.Build();

                //for MVC Controller Set the dependency resolver implementation.
                var resolverMvc = new AutofacDependencyResolver(container);
                System.Web.Mvc.DependencyResolver.SetResolver(resolverMvc);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}