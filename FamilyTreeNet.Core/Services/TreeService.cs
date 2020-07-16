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

        public Task DeleteIndividualById(long id)
            => this.individualRepository.MarkIndividualAsDeleted(id); 

        public Task<List<FamilyDto>> GetSpouseFamiliesByIndividualId(long id, bool includeDeleted) 
            => this.familyRepository.GetSpouseFamiliesByIndividualId(id, includeDeleted);

        public Task<FamilyDto> GetFamilyById(long id)
            => this.familyRepository.GetFamilyById(id);

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

        public Task AddSpouse(long sfam, long id) => this.familyRepository.AddSpouse(sfam, id);

        public Task AddChild(long cfam, long id) => this.familyRepository.AddChild(cfam, id);

        public Task<long> AddPerson(IndividualDto indi)
        {
            if (indi is null)
            {
                throw new ArgumentNullException(nameof(indi));
            }

            return AddPersonImpl(indi);

            async Task<long> AddPersonImpl(IndividualDto ind)
            {
                await this.individualRepository.AddOrUpdate(ind).ConfigureAwait(false);
                return indi.Id;
            }
        }

        public async Task Load(Stream gedcom)
        {
            var rdr = new GedcomFileReader(this);
            await rdr.ReadFile(gedcom).ConfigureAwait(false);
        }

        public Task Update(IndividualDto individual)
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

            async Task UpdateImpl(IndividualDto indi)
            {
                await this.individualRepository.AddOrUpdate(indi).ConfigureAwait(false);

            }
        }

        public Task Update(FamilyDto family)
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

            async Task UpdateImpl(FamilyDto fam)
            {
                await this.familyRepository.AddOrUpdate(fam).ConfigureAwait(false);
            }
        }


        internal Task UpdateRelations(long id, List<long> spouses, List<long> children)
        {
            return this.familyRepository.UpdateRelations(id, spouses, children);
        }
    }
}
