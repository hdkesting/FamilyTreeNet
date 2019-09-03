using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin.Init
{
    public class LoadModel : PageModel
    {
        private readonly TreeService treeService;

        public LoadModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            // no action required
        }

        public async Task<IActionResult> OnPost(string sure)
        {
            if (String.Equals(sure, "yes", StringComparison.OrdinalIgnoreCase))
            {
                await this.treeService.DeleteAll();
                this.Message = "The database has been rewritten.";
            }

            return RedirectToPage("./Index");
        }

    }
}