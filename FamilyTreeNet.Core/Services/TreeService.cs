using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Interfaces;
using System;
using System.Collections.Generic;
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
    }
}
