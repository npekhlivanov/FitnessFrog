using Owin;

namespace Treehouse.FitnessFrog
{
    public partial class Startup 
        // follow the convention of locating our authentication configuration in the "App_Start" folder while keeping the main "Startup.cs" code file in the root of the project
    {
        public void Configuration(IAppBuilder app)
        {
            // configure pipeline's middleware components
            ConfigureAuth(app);
        }
    }
}