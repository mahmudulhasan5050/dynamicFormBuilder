using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormWizard.Pages.Countries
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DeleteModel(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }
        [BindProperty]
        public Country country { get; set; }
        public IFormFile file { get; set; }
        public void OnGet(int Id)
        {
            country = _db.Country.FirstOrDefault(u => u.Id == Id);
        }

        public IActionResult OnPost()
        {
            var countryFromDb = _db.Country.Find(country.Id);

            if (countryFromDb != null)
            {
                
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var oldImagePath = Path.Combine(wwwRootPath, countryFromDb.FlagImage.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                   System.IO.File.Delete(oldImagePath);
                }

                _db.Country.Remove(countryFromDb);
                _db.SaveChanges();
                TempData["success"] = "Country has been deleted.";
                return RedirectToPage("Index");
            }
            TempData["error"] = "Delete is not possible.";
            return Page();
        }
    }
}
