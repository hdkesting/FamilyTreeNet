using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FamilyTree.Infra.Models
{
    // only used for migrations
    internal class FamilyTreeContextFactory : IDesignTimeDbContextFactory<FamilyTreeContext>
    {
        public FamilyTreeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FamilyTreeContext>();
            optionsBuilder.UseSqlServer(@"Server=.\sqlexpress;Database=familytree;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;Application Name=FamilyTreeNet");

            return new FamilyTreeContext(optionsBuilder.Options);
        }
    }
}
