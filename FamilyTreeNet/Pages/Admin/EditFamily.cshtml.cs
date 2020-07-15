using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    [Authorize(Roles = "editor")]
    public class EditFamilyModel : PageModel
    {
        public void OnGet()
        {
            // TODO
        }
    }
}