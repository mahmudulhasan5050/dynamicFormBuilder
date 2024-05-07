using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.Questions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;            
        }
        [BindProperty]
        public Question question { get; set; }
        public int questionCountFromDb { get; set; }
        [BindProperty(SupportsGet = true)]
        public int myFormId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int counter { get; set; }

        public void OnGet(int myformid)
        {
            myFormId = myformid;
            questionCountFromDb = _db.Questions.Where(x => x.MyFormId == myFormId && x.IsInUse == true).ToList().Count();
            counter = questionCountFromDb + 1;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                questionCountFromDb = _db.Questions.Where(x => x.MyFormId == myFormId && x.IsInUse == true).ToList().Count();
                counter = questionCountFromDb + 1;
            }
            question.CreatedAt = question.UpdatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Questions.Add(question);
                _db.SaveChanges();
                TempData["success"] = "New Question has been created.";
                return RedirectToPage("Index", new { myformid = myFormId });
            }
            return Page();
        }

    }
}
