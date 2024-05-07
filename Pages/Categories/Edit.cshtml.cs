using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FormWizard.Pages.Categories
{
    [DbContext(typeof(Data.ApplicationDbContext))]

    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Category category { get; set; }
        public void OnGet(int Id)
        {
            category = _db.Category.FirstOrDefault(u=>u.Id == Id);
        }

        public IActionResult OnPost()
        {
            category.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category Item has been updated.";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
