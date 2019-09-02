using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FamilyTree.Infra.Models
{
    internal class FamilyTreeContextFactory : IDesignTimeDbContextFactory<FamilyTreeContext>
    {
        public FamilyTreeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FamilyTreeContext>();
            optionsBuilder.UseSqlServer(StartupInfra.ConnectionString);

            return new FamilyTreeContext(optionsBuilder.Options);
        }
    }
}
