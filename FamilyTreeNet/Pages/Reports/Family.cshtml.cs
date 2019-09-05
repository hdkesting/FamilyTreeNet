﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using FamilyTreeNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Reports
{
    [AllowAnonymous]
    public class FamilyModel : PageModel
    {
        private readonly TreeService treeService;

        public FamilyModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public string Lastname { get; set; }

        public List<IndividualVm> Individuals { get; } = new List<IndividualVm>();

        public async Task OnGet(string name)
        {
            this.Lastname = name;
            var list = await this.treeService.GetIndividualsByLastname(name).ConfigureAwait(false);

            this.Individuals.Clear();
            this.Individuals.AddRange(list.Select(i => new IndividualVm(i)));
 
        }
    }
}