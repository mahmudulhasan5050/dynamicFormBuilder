using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.MyForms
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public MyForm myForm { get; set; }
        public IActionResult OnGet(int? myformid)
        {
            if (myformid == null)
            {
                return NotFound();
            }

            myForm = _db.MyForms.Include(u => u.Category).Include(c => c.Country).AsNoTracking().FirstOrDefault(o=>o.Id == myformid);
            if (myForm == null) {
                return NotFound();
            }

            var category = _db.Category;
            var country = _db.Country;
            if (category != null && country != null)
            {
                IEnumerable<SelectListItem> categoryList = category.Select(
                    u=> new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                    );
                IEnumerable<SelectListItem> countryList = country.Select(
                    u => new SelectListItem
                    {
                       Text = u.Name,
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
            myForm.UpdatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.MyForms.Update(myForm);
                _db.SaveChanges();
                TempData["success"] = "Form has been updated.";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
