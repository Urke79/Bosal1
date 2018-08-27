using DataAccess.Entities;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Web.Http;
using Unity.WebApi;

namespace BosalMontaze
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterTypes(
                        AllClasses.FromLoadedAssemblies(),
                        WithMappings.FromMatchingInterface,
                        WithName.Default);

            container.RegisterType<DbContext, BosalMontazeDbContext>(new PerThreadLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}