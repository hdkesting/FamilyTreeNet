using FamilyTree.Infra.MySql.Repositories;
using FamilyTree.Infra.MySql.Support;

using FamilyTreeNet.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyTree.Infra.MySql
{
    public static class StartupInfra
    {
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-3.1

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IIndividualRepository, IndividualRepository>();
            services.AddTransient<IFamilyRepository, FamilyRepository>();

            services.AddSingleton(new MySqlDbOptions 
                    { 
                        ConnectionString = configuration.GetConnectionString("MySqlConnection") 
                    });
        }
    }
}
