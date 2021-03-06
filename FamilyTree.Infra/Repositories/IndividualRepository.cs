﻿using FamilyTree.Infra.Models;

using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if (individual is null)
            {
                throw new System.ArgumentNullException(nameof(individual));
            }

            var indi = individual.Id == 0 ? null : await this.context.Individuals.FirstOrDefaultAsync(i => i.Id == individual.Id).ConfigureAwait(false);

            if (indi == null)
            {
                if (individual.Id == 0)
                {
                    individual.Id = (await this.context.Individuals.Select(i => i.Id).MaxAsync().ConfigureAwait(false)) + 1;
                }

                indi = new Individual { Id = individual.Id };
                this.context.Individuals.Add(indi);
            }

            indi.IsDeleted = false;
            indi.Firstnames = individual.Firstnames;
            indi.Lastname = individual.Lastname;
            // C#8: switch expression
            indi.Sex = individual.Sex == FamilyTreeNet.Core.Support.Sex.Male ? 'M'
                    : individual.Sex == FamilyTreeNet.Core.Support.Sex.Female ? 'F'
                    : '?';

            indi.BirthDateInt = individual.BirthDate?.ToInt32();
            indi.BirthPlace = individual.BirthPlace;
            indi.DeathDateInt = individual.DeathDate?.ToInt32();
            indi.DeathPlace = individual.DeathPlace;

            await this.context.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<int> Count(bool includeDeleted) =>
            this.context.Individuals.CountAsync(f => includeDeleted || !f.IsDeleted);

        public async Task DeleteAll()
        {
            var sql = "DELETE FROM " + this.context.Individuals.GetTableName();
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            await this.context.Database.ExecuteSqlRawAsync(sql).ConfigureAwait(false);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
        }

        public async Task<IndividualDto> GetById(long id, bool includeDeleted)
        {
            return Map(await this.context.Individuals
                .FirstOrDefaultAsync(i => i.Id == id && (includeDeleted || !i.IsDeleted))
                .ConfigureAwait(false));
        }

        public async Task<List<IndividualDto>> GetIndividualsByLastname(string name)
        {
            var dblist= await this.context.Individuals
                .Where(i => !i.IsDeleted)
                .Where(i => i.Lastname == name)
                .ToListAsync()
                .ConfigureAwait(false);

            return dblist.Select(Map).ToList();
        }

        public Task<List<NameCount>> GetLastNames()
        {
            return this.context.Individuals
                .GroupBy(i => i.Lastname)
                .Select(g => new NameCount(g.Key, g.Count()))
                .ToListAsync();
        }

        public Task<int> GetTotalChildrenCount() =>
            this.context.Individuals
            .Where(i => !i.IsDeleted)
            .CountAsync(i => i.ChildFamilies.Count != 0);

        public Task<int> GetTotalSpouseCount() =>
            this.context.Individuals
            .Where(i => !i.IsDeleted)
            .CountAsync(i => i.SpouseFamilies.Count != 0);

        public async Task MarkIndividualAsDeleted(long id)
        {
            var indi = await this.context.Individuals.SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);

            if (indi != null)
            {
                indi.IsDeleted = true;
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<IndividualDto>> SearchByName(string firstname, string lastname)
        {
            var qry = this.context.Individuals
                .Where(i => !i.IsDeleted);

            firstname = firstname?.Trim();
            lastname = lastname?.Trim();

            if (!string.IsNullOrEmpty(firstname))
            {
                foreach(var part in firstname.Split(new [] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    qry = qry.Where(i => i.Firstnames.Contains(part));
                }
            }

            if (!string.IsNullOrEmpty(lastname))
            {
                qry = qry.Where(i => i.Lastname.Contains(lastname));
            }

            var list = await qry.ToListAsync().ConfigureAwait(false);

            return list.Select(Map);
        }

        private IndividualDto Map(Individual indi)
        {
            return new IndividualDto
            {
                Id = indi.Id,
                Firstnames = indi.Firstnames,
                Lastname = indi.Lastname,
                BirthDate = indi.BirthDateInt == null ? null : new FamilyTreeNet.Core.Support.GeneaDate(indi.BirthDateInt.Value),
                BirthPlace = indi.BirthPlace,
                DeathDate = indi.DeathDateInt == null ? null : new FamilyTreeNet.Core.Support.GeneaDate(indi.DeathDateInt.Value),
                DeathPlace = indi.DeathPlace,
                Sex = indi.Sex == 'M' ? FamilyTreeNet.Core.Support.Sex.Male
                    : indi.Sex == 'F' ? FamilyTreeNet.Core.Support.Sex.Female
                    : FamilyTreeNet.Core.Support.Sex.Unknown,
            };
        }
    }
}
