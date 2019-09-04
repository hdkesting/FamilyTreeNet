using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin.Init
{
    public class LoadModel : PageModel
    {
        private readonly TreeService treeService;
        private readonly IHostingEnvironment hostingEnvironment;

        public LoadModel(TreeService treeService, IHostingEnvironment hostingEnvironment)
        {
            this.treeService = treeService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> OnPost(string sure)
        {
            if (String.Equals(sure, "yes", StringComparison.OrdinalIgnoreCase))
            {
                var path = System.IO.Path.Combine(hostingEnvironment.ContentRootPath, "Resources/sampleFamily.ged");
                using (var gedcom = System.IO.File.OpenRead(path))
                {
                    await this.treeService.Load(gedcom);
                }

                this.Message = "The database has been rewritten.";
            }

            return RedirectToPage("./Index");
        }

    }
}