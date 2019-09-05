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

        public Task<List<NameCount>> GetLastNames() 
            => this.individualRepository.GetLastNames();

        public Task<IndividualDto> GetIndividualById(long id) 
            => this.individualRepository.GetById(id, false);

        public Task<IEnumerable<IndividualDto>> SearchByName(string firstname, string lastname)
            => this.individualRepository.SearchByName(firstname, lastname);

        public async Task DeleteAll()
        {
            await this.familyRepository.DeleteAll().ConfigureAwait(false);
            await this.individualRepository.DeleteAll().ConfigureAwait(false);
        }

        public Task<List<FamilyDto>> GetSpouseFamiliesByIndividualId(long id, bool includeDeleted) 
            => this.familyRepository.GetSpouseFamiliesByIndividualId(id, includeDeleted);

        public Task<List<FamilyDto>> GetChildFamiliesByIndividualId(long id, bool includeDeleted)
            => this.familyRepository.GetChildFamiliesByIndividualId(id, includeDeleted);

        public Task<List<IndividualDto>> GetIndividualsByLastname(string name) 
            => this.individualRepository.GetIndividualsByLastname(name);

        public async Task<Summary> GetCountSummary()
        {
            var result = new Summary
            {
                FamilyCount = await this.familyRepository.Count(true).ConfigureAwait(false),
                IndividualCount = await this.individualRepository.Count(true).ConfigureAwait(false),
                ChildCount = await this.individualRepository.GetTotalChildrenCount().ConfigureAwait(false),
                SpouseCount = await this.individualRepository.GetTotalSpouseCount().ConfigureAwait(false)
            };

            return result;
        }

        public async Task Load(Stream gedcom)
        {
            var rdr = new GedcomFileReader(this);
            await rdr.ReadFile(gedcom).ConfigureAwait(false);
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
            await this.individualRepository.AddOrUpdate(individual).ConfigureAwait(false);

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
            await this.familyRepository.AddOrUpdate(family).ConfigureAwait(false);
        }

        internal Task UpdateRelations(long id, List<long> spouses, List<long> children)
        {
            return this.familyRepository.UpdateRelations(id, spouses, children);
        }
    }
}
