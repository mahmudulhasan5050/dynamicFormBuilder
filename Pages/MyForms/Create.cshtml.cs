using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.MyForms
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public MyForm myForm { get; set; }
        [BindProperty(SupportsGet =true)]
        public IEnumerable<SelectListItem> categoryList { get; set; }
        [BindProperty(SupportsGet = true)]
        public IEnumerable<SelectListItem> countryList { get; set; }

        public IActionResult OnGet()
        {
            var categoryFromDb = _db.Category;
            var countryFromDb = _db.Country;

            if (categoryFromDb != null && countryFromDb != null)
            {
                 categoryList = categoryFromDb.Select(u=>
                new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                ).ToList();
                countryList = countryFromDb.Select(u =>
                new SelectListItem
                {
                    Text= u.Name,
                    Value = u.Id.ToString()
                }

                );
                ViewData["categoryList"] = categoryList;
                ViewData["countryList"] = countryList;

            }
            return Page();

        }

        public IActionResult OnPost()
        {
            myForm.CreatedAt = myForm.UpdatedAt = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewData["categoryList"] = categoryList;
                ViewData["countryList"] = countryList;
                return Page();
            }
            _db.MyForms.Add(myForm);
            _db.SaveChanges();
            TempData["success"] = "New Form has been created.";
            return RedirectToPage("Index");

        }
    }
}
