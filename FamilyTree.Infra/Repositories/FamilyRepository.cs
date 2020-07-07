using FamilyTree.Infra.Models;
using FamilyTreeNet.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Dto;

namespace FamilyTree.Infra.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly FamilyTreeContext context;
        private readonly IIndividualRepository individualRepository;

        public FamilyRepository(FamilyTreeContext context, IIndividualRepository individualRepository)
        {
            this.context = context;
            this.individualRepository = individualRepository;
        }

        public async Task AddChild(long cfam, long id)
        {
            var fam = await this.context.Families
                .Include(f => f.Children)
                .SingleOrDefaultAsync(f => f.Id == cfam).ConfigureAwait(false);
            var child = await this.context.Individuals.SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);

            if (fam != null && child != null && !fam.Children.Any(c => c.ChildId == id))
            {
                fam.Children.Add(new ChildRelation() { ChildFamilyId = cfam, ChildId = id });
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public Task AddOrUpdate(FamilyDto family)
        {
            if (family is null)
            {
                throw new System.ArgumentNullException(nameof(family));
            }

            return AddOrUpdateImpl(family);

            async Task AddOrUpdateImpl(FamilyDto fam2)
            {
                var fam = fam2.Id == 0 ? null : await this.context.Families.FirstOrDefaultAsync(f => f.Id == fam2.Id).ConfigureAwait(false);

                if (fam == null)
                {
                    if (fam2.Id == 0)
                    {
                        fam2.Id = (await this.context.Families.Select(i => i.Id).MaxAsync().ConfigureAwait(false)) + 1;
                    }

                    fam = new Family { Id = fam2.Id };
                    this.context.Families.Add(fam);
                }

                fam.IsDeleted = false;
                fam.MarriageDateInt = fam2.MarriageDate?.ToInt32();
                fam.MarriagePlace = fam2.MarriagePlace;
                fam.DivorceDateInt = fam2.DivorceDate?.ToInt32();
                fam.DivorcePlace = fam2.DivorcePlace;

                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task AddSpouse(long sfam, long id)
        {
            var fam = await this.context.Families
               .Include(f => f.Spouses)
               .SingleOrDefaultAsync(f => f.Id == sfam).ConfigureAwait(false);
            var spouse = await this.context.Individuals.SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);

            if (fam != null && spouse != null && !fam.Spouses.Any(s => s.SpouseId == id))
            {
                fam.Spouses.Add(new SpouseRelation() { SpouseFamilyId = sfam, SpouseId = id });
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public Task<int> Count(bool includeDeleted) =>
            this.context.Families.CountAsync(f => includeDeleted || !f.IsDeleted);

        public async Task DeleteAll()
        {
            // this executes 1 "select all" command, plus 1 large "delete all separate records" command
            //// this.context.Families.RemoveRange(this.context.Families);
            //// await this.context.SaveChangesAsync();
            
            var sql = "DELETE FROM " + this.context.Families.GetTableName();
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            await this.context.Database.ExecuteSqlCommandAsync(sql).ConfigureAwait(false);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
        }

        public async Task<List<FamilyDto>> GetChildFamiliesByIndividualId(long id, bool includeDeleted)
        {
            var list = await this.context.Families
                .Include(f => f.Spouses)
                .Include(f => f.Children)
                .Where(f => includeDeleted || !f.IsDeleted)
                .Where(f => f.Children.Any(s => s.ChildId == id))
                .ToListAsync()
                .ConfigureAwait(false);

            var res = new List<FamilyDto>();
            foreach (var fam in list)
            {
                res.Add(await this.Map(fam, includeDeleted).ConfigureAwait(false));
            }

            return res;
        }

        public async Task<List<FamilyDto>> GetSpouseFamiliesByIndividualId(long id, bool includeDeleted)
        {
            var list = await this.context.Families
                .Include(f => f.Spouses)
                .Include(f => f.Children)
                .Where(f => includeDeleted || !f.IsDeleted)
                .Where(f => f.Spouses.Any(s => s.SpouseId == id))
                .ToListAsync()
                .ConfigureAwait(false);

            var res = new List<FamilyDto>();
            foreach (var fam in list)
            {
                res.Add(await this.Map(fam, includeDeleted).ConfigureAwait(false));
            }

            return res;
        }

        public async Task UpdateRelations(long familyId, List<long> spouses, List<long> children)
        {
            if (spouses is null)
            {
                spouses = new List<long>();
            }

            if (children is null)
            {
                children = new List<long>();
            }

            var fam = await this.context.Families
                .Include(f => f.Spouses)
                .Include(f => f.Children)
                .FirstOrDefaultAsync(f => f.Id == familyId)
                .ConfigureAwait(false);

            if (fam != null)
            {
                fam.Spouses.Clear();
                foreach (var spouseId in spouses)
                {
                    fam.Spouses.Add(new SpouseRelation { SpouseFamilyId = familyId, SpouseId = spouseId });
                }

                fam.Children.Clear();
                foreach(var childId in children)
                {
                    fam.Children.Add(new ChildRelation { ChildFamilyId = familyId, ChildId = childId });
                }

                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }

            // else just ignore
        }

        private async Task<FamilyDto> Map(Family db, bool includeDeleted)
        {
            var res = new FamilyDto
            {
                Id = db.Id,
                MarriageDate = db.MarriageDateInt == null ? null : new FamilyTreeNet.Core.Support.GeneaDate(db.MarriageDateInt.Value),
                MarriagePlace = db.MarriagePlace,
                DivorceDate = db.DivorceDateInt == null ? null : new FamilyTreeNet.Core.Support.GeneaDate(db.DivorceDateInt.Value),
                DivorcePlace = db.DivorcePlace,
            };

            foreach (var spouse in db.Spouses)
            {
                res.Spouses.Add(await this.individualRepository.GetById(spouse.SpouseId, includeDeleted).ConfigureAwait(false));
            }

            foreach(var child in db.Children)
            {
                res.Children.Add(await this.individualRepository.GetById(child.ChildId, includeDeleted).ConfigureAwait(false));
            }

            return res;
        }
    }
}
