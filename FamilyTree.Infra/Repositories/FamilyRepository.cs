using FamilyTree.Infra.Models;
using FamilyTreeNet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FamilyTree.Infra.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly FamilyTreeContext context;

        public FamilyRepository(FamilyTreeContext context)
        {
            this.context = context;
        }

        public Task<int> Count(bool includeDeleted) =>
            this.context.Families.CountAsync(f => includeDeleted || !f.IsDeleted);

        public async Task DeleteAll()
        {
            this.context.Families.RemoveRange(this.context.Families);
            await this.context.SaveChangesAsync();
        }

    }
}
