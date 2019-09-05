using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Reports
{
    [AllowAnonymous]
    public class SummaryModel : PageModel
    {
        private readonly TreeService treeService;

        public SummaryModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public int Individuals { get; private set; }
        public int Families { get; private set; }
        public int Spouses { get; private set; }
        public int Children { get; private set; }

        public async Task OnGet()
        {
            var summary = await this.treeService.GetCountSummary().ConfigureAwait(false);

            this.Individuals = summary.IndividualCount;
            this.Families = summary.FamilyCount;
            this.Spouses = summary.SpouseCount;
            this.Children = summary.ChildCount;
        }
    }
}