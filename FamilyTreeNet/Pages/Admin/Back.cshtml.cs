
using FamilyTreeNet.Support;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    public class BackModel : PageModel
    {
        public IActionResult OnGet()
        {
            var back = PageStack.GoBack(HttpContext.Session);
            if (back is null)
            {
                return RedirectToPage("Index");
            }

            return Redirect(back.Url);
        }
    }
}