using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeNet.Core.Services
{
    public class TreeService
    {
        private readonly IIndividualRepository individualRepository;
        private readonly IFamilyRepository familyRepository;

        public TreeService(IIndividualRepository individualRepository, IFamilyRepository familyRepository)
        {
            this.individualRepository = individualRepository;
            this.familyRepository = familyRepository;
        }

        public async Task DeleteAll()
        {
            await this.familyRepository.DeleteAll().ConfigureAwait(false);
            await this.individualRepository.DeleteAll();
        }

        public async Task<Summary> GetCountSummary()
        {
            var result = new Summary();
            result.FamilyCount = await this.familyRepository.Count(true);
            result.IndividualCount = await this.individualRepository.Count(true);
            result.ChildCount = await this.individualRepository.GetTotalChildrenCount();
            result.SpouseCount = await this.individualRepository.GetTotalSpouseCount();

            return result;
        }

        public async Task Load(Stream gedcom)
        {
            var rdr = new GedcomFileReader(this);
            await rdr.ReadFile(gedcom);
        }

        internal Task Update(IndividualDto individual)
        {
            return Task.CompletedTask;
        }

        internal Task Update(FamilyDto family)
        {
            return Task.CompletedTask;
        }

        internal Task UpdateRelations(long id, List<long> spouses, List<long> children)
        {
            return Task.CompletedTask;
        }
    }
}
