using System;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin.Init
{
    [Authorize(Roles = "admin")]
    public class ClearModel : PageModel
    {
        private readonly TreeService treeService;

        public ClearModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> OnPost(string sure)
        {
            if (String.Equals(sure, "yes", StringComparison.OrdinalIgnoreCase))
            {
                await this.treeService.DeleteAll().ConfigureAwait(false);
                this.Message = "The database has been cleared.";
            }

            return RedirectToPage("./Index");
        }
    }
}