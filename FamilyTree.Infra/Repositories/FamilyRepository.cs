using FamilyTree.Infra.Models;
using FamilyTreeNet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task AddOrUpdate(FamilyDto family)
        {
            var fam = await this.context.Families.FirstOrDefaultAsync(f => f.Id == family.Id);

            if (fam == null)
            {
                fam = new Family { Id = family.Id };
                this.context.Families.Add(fam);
            }

            fam.IsDeleted = false;
            fam.MarriageDate = family.MarriageDate;
            fam.MarriagePlace = family.MarriagePlace;
            fam.DivorceDate = family.DivorceDate;
            fam.DivorcePlace = family.DivorcePlace;

            await this.context.SaveChangesAsync();
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
            await this.context.Database.ExecuteSqlCommandAsync(sql);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
        }

        public async Task<List<FamilyDto>> GetChildFamiliesByIndividualId(long id, bool includeDeleted)
        {
            var list = await this.context.Families
                .Include(f => f.Spouses)
                .Include(f => f.Children)
                .Where(f => includeDeleted || !f.IsDeleted)
                .Where(f => f.Children.Any(s => s.ChildId == id))
                .ToListAsync();

            var res = new List<FamilyDto>();
            foreach (var fam in list)
            {
                res.Add(await Map(fam, includeDeleted));
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
                .ToListAsync();

            var res = new List<FamilyDto>();
            foreach (var fam in list)
            {
                res.Add(await Map(fam, includeDeleted));
            }

            return res;
        }

        public async Task UpdateRelations(long familyId, List<long> spouses, List<long> children)
        {
            var fam = await this.context.Families
                .Include(f => f.Spouses)
                .Include(f => f.Children)
                .FirstOrDefaultAsync(f => f.Id == familyId);

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

                await this.context.SaveChangesAsync();
            }

            // else just ignore
        }

        private async Task<FamilyDto> Map(Family db, bool includeDeleted)
        {
            var res = new FamilyDto
            {
                Id = db.Id,
                MarriageDate = db.MarriageDate,
                MarriagePlace = db.MarriagePlace,
                DivorceDate = db.DivorceDate,
                DivorcePlace = db.DivorcePlace,
            };

            foreach (var spouse in db.Spouses)
            {
                res.Spouses.Add(await this.individualRepository.GetById(spouse.SpouseId, includeDeleted));
            }

            foreach(var child in db.Children)
            {
                res.Children.Add(await this.individualRepository.GetById(child.ChildId, includeDeleted));
            }

            return res;
        }
    }
}
