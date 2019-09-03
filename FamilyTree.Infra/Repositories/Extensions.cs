using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;

namespace FamilyTree.Infra.Repositories
{
    internal static class Extensions
    {
        public static string GetTableName<T>(this DbSet<T> dbSet) where T : class
        {
            var model = dbSet.GetDbContext().Model;
            var entityTypes = model.GetEntityTypes();
            var entityType = entityTypes.First(t => t.ClrType == typeof(T));
            var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
            var tableName = tableNameAnnotation.Value.ToString();
            return tableName;
        }

        // How to use:
        // 
        // class MyDbContext : DbContext {
        //    public DbSet<FooBar> FooBars { get; set; }
        // }
        // ...
        // var myContext = new MyDbContext();
        // var tableName = myContext.GetTableName(myContext.FooBars);
        //                                  ~~~~~~~~~~~~~~
        // tableName; // -> "FooBars"
        // https://dev.to/j_sakamoto/how-to-get-the-actual-table-name-from-dbset-in-entityframework-core-20-56k0


        public static DbContext GetDbContext<T>(this DbSet<T> dbSet) where T : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext))
                                       as ICurrentDbContext;
            return currentDbContext.Context;
        }
        // https://dev.to/j_sakamoto/how-to-get-a-dbcontext-from-a-dbset-in-entityframework-core-c6m
    }
}
