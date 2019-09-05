﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using FamilyTreeNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    public class SearchModel : PageModel
    {
        private readonly TreeService treeService;

        public SearchModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public List<IndividualVm> List { get; } = new List<IndividualVm>();

        public async Task OnGet(string firstname, string lastname)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;

            if (!string.IsNullOrWhiteSpace(Firstname) || !string.IsNullOrWhiteSpace(Lastname))
            {
                var qry = await this.treeService.SearchByName(Firstname, Lastname).ConfigureAwait(false);
                this.List.AddRange(qry.Select(i => new IndividualVm(i)));
            }
        }

        public IActionResult OnPost(string firstname, string lastname)
        {
            return RedirectToPage("Search", new { firstname, lastname });
        }
    }
}