using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.Countries
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EditModel(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Country country { get; set; }
        public void OnGet(int Id, IFormFile? file)
        {
            country = _db.Country.FirstOrDefault(u=>u.Id == Id);
        }

        public async Task<IActionResult> OnPost(Country country, IFormFile? file)
        {
            country.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\flags");
                    var extension = Path.GetExtension(file.FileName);

                    if (country.FlagImage != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, country.FlagImage.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    country.FlagImage = @"\images\flags\" + fileName + extension;
                }



                _db.Country.Update(country);
                await _db.SaveChangesAsync();
                TempData["success"] = "Country Edited successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
