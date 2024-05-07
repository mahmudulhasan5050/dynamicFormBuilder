using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.Countries
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CreateModel(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
           _hostEnvironment = hostEnvironment;
        }
        [BindProperty]
        public Country country { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost(Country country, IFormFile? file)
        {
            country.CreatedAt = country.UpdatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (!_db.Country.Any(o => o.Name == country.Name))
                {
                    if (file != null)
                    {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\flags");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                        country.FlagImage = @"\images\flags\" + fileName + extension;
                    }


                    _db.Country.Add(country);
                    _db.SaveChanges();
                    TempData["success"] = "Country has been created.";
                    return RedirectToPage("Index");
                }
                TempData["error"] = "Country name is already existed.";
            }

            return Page();
        }
    }
}
