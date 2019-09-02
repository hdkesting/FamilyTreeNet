using FamilyTree.Infra.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTree.Infra
{
    public static class StartupInfra
    {
        public const string ConnectionString = @"Server=.\sqlexpress;Database=familytree;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;Application Name=FamilyTreeNet";

        public static void ConfigureServices(IServiceCollection services)
        {
            // TODO get from configuration
            services.AddDbContext<FamilyTreeContext>(op => op.UseSqlServer(ConnectionString));
        }
    }
}
