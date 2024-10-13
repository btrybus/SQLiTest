using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SQLiTest.Pages
{
    public class WelcomeModel : PageModel
    {
        [BindProperty]
        public required string UserName { get; set; }


        public void OnGet(string username)
        {
            UserName = username;
        }

    }
}
