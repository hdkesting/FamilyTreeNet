using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin.Init
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        public const string MessageKey = "message";

        private readonly TreeService treeService;

        public IndexModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public int Individuals { get; private set; }

        public int Families { get; private set; }

        [TempData]
        public string Message { get; set; }

        public async Task OnGet()
        {
            var summ = await this.treeService.GetCountSummary().ConfigureAwait(false);
            this.Individuals = summ.IndividualCount;
            this.Families = summ.FamilyCount;
        }
    }
}