using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Reports
{
    public class FamilyNamesModel : PageModel
    {
        private readonly TreeService treeService;

        public FamilyNamesModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public IEnumerable<NameCount> Names { get; private set; } 

        public async Task OnGet()
        {
            this.Names = await this.treeService.GetLastNames();
        }
    }
}