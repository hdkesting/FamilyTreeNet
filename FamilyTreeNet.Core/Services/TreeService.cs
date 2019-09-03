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
            var result = new Summary
            {
                FamilyCount = await this.familyRepository.Count(true),
                IndividualCount = await this.individualRepository.Count(true),
                ChildCount = await this.individualRepository.GetTotalChildrenCount(),
                SpouseCount = await this.individualRepository.GetTotalSpouseCount()
            };

            return result;
        }

        public async Task Load(Stream gedcom)
        {
            var rdr = new GedcomFileReader(this);
            await rdr.ReadFile(gedcom);
        }

        internal Task Update(IndividualDto individual)
        {
            if (individual is null)
            {
                throw new ArgumentNullException(nameof(individual));
            }

            if (individual.Id <= 0)
            {
                throw new ArgumentException("ID must be greater than zero.");
            }

            return UpdateImpl(individual);
        }

        private async Task UpdateImpl(IndividualDto individual)
        {
            await this.individualRepository.AddOrUpdate(individual);

        }

        internal Task Update(FamilyDto family)
        {
            if (family is null)
            {
                throw new ArgumentNullException(nameof(family));
            }

            if (family.Id <= 0)
            {
                throw new ArgumentException("ID must be greater than zero.");
            }

            return UpdateImpl(family);
        }

        private async Task UpdateImpl(FamilyDto family)
        {
            await this.familyRepository.AddOrUpdate(family);
        }

        internal Task UpdateRelations(long id, List<long> spouses, List<long> children)
        {
            return this.familyRepository.UpdateRelations(id, spouses, children);
        }
    }
}
