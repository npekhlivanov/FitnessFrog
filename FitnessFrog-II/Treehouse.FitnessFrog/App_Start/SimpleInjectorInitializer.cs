[assembly: WebActivator.PostApplicationStartMethod(typeof(Treehouse.FitnessFrog.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace Treehouse.FitnessFrog.App_Start
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using Treehouse.FitnessFrog.Shared.Data;
    using Treehouse.FitnessFrog.Shared.Models;
    using Treehouse.FitnessFrog.Shared.Security;

    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle(); // each web request will be implicitly defined as a scope

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static void InitializeContainer(Container container)
        {
            // Classes will be instantiated at the beginning of a request and shared across any objects servicing the request. 
            // When the request ends, the container will dispose of the instance.
            container.Register<Context>(Lifestyle.Scoped);
            container.Register<EntriesRepository>(Lifestyle.Scoped);
            container.Register<ActivitiesRepository>(Lifestyle.Scoped);

            // Indentity configuration
            container.Register<ApplicationUserManager>(Lifestyle.Scoped);
            container.Register<ApplicationSignInManager>(Lifestyle.Scoped);

            // The AccountController class needs a reference to the concrete implementation of Identity's IAuthenticationManager interface
            // We will not rely upon the DI container to instantiate an instance of the concrete implementation or directly instantiate the class ourselves
            container.Register(() => // retrieve the instance from the current OWIN context
                container.IsVerifying  
                ? new OwinContext().Authentication // create our OWIN context instance when the DI container is being verified
                : HttpContext.Current.GetOwinContext().Authentication,
                Lifestyle.Scoped);

            // To get an instance of the Context class for the UserStore<User> class constructor, we can use the DI container's GetInstance generic method
            container.Register<IUserStore<User>>(() => new UserStore<User>(container.GetInstance<Context>()),
                Lifestyle.Scoped);
        }
    }
}