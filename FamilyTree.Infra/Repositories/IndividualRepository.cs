using FamilyTree.Infra.Models;
using FamilyTreeNet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FamilyTreeNet.Core.Dto;

namespace FamilyTree.Infra.Repositories
{
    public class IndividualRepository : IIndividualRepository
    {
        private readonly FamilyTreeContext context;

        public IndividualRepository(FamilyTreeContext context)
        {
            this.context = context;
        }

        public async Task AddOrUpdate(IndividualDto individual)
        {
            var indi = await this.context.Individuals.FirstOrDefaultAsync(i => i.Id == individual.Id);

            if (indi == null)
            {
                indi = new Individual { Id = individual.Id };
                this.context.Individuals.Add(indi);
            }

            indi.IsDeleted = false;
            indi.Firstnames = individual.Firstnames;
            indi.Lastname = individual.Lastname;
            indi.BirthDate = individual.BirthDate;
            indi.BirthPlace = individual.BirthPlace;
            indi.DeathDate = individual.DeathDate;
            indi.DeathPlace = individual.DeathPlace;

            await this.context.SaveChangesAsync();
        }

        public Task<int> Count(bool includeDeleted) =>
            this.context.Individuals.CountAsync(f => includeDeleted || !f.IsDeleted);

        public async Task DeleteAll()
        {
            var sql = "DELETE FROM " + this.context.Individuals.GetTableName();
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            await this.context.Database.ExecuteSqlCommandAsync(sql);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
        }

        public Task<int> GetTotalChildrenCount() =>
            this.context.Individuals
            .Where(i => !i.IsDeleted)
            .CountAsync(i => i.ChildFamilies.Count != 0);

        public Task<int> GetTotalSpouseCount() =>
            this.context.Individuals
            .Where(i => !i.IsDeleted)
            .CountAsync(i => i.SpouseFamilies.Count != 0);
    }
}
