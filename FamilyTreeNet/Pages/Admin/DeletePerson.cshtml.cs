using System.Threading.Tasks;

using FamilyTreeNet.Core.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    [Authorize(Roles = "editor")]
    public class DeletePersonModel : PageModel
    {
        private readonly TreeService treeService;

        public DeletePersonModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public long Id { get; private set; }

        public string FullName { get; private set; }

        public async Task<IActionResult> OnGet(long id)
        {
            var indi = await this.treeService.GetIndividualById(id).ConfigureAwait(false);

            if (indi == null)
            {
                return RedirectToPage("Search");
            }

            this.Id = id;
            this.FullName = $"{indi.Firstnames} {indi.Lastname}";

            return Page();
        }

        public async Task<IActionResult> OnPost(long id, long confirmid = -1)
        {
            if (id >= 0 && id == confirmid)
            {
                await this.treeService.DeleteIndividualById(id).ConfigureAwait(false);
            }

            return RedirectToPage("Search");
        }
    }
}