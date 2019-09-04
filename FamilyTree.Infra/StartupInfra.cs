using FamilyTree.Infra.Models;
using FamilyTree.Infra.Repositories;
using FamilyTreeNet.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyTree.Infra
{
    public static class StartupInfra
    {
        public static void ConfigureServices(IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddDbContext<FamilyTreeContext>(op => op.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IFamilyRepository, FamilyRepository>();
            services.AddTransient<IIndividualRepository, IndividualRepository>();
        }
    }
}
