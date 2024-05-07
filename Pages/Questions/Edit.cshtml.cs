using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.Questions
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Question question { get; set; }
        public void OnGet(int questionid, int myformid)
        {
            question = _db.Questions.Find(questionid);
            ViewData["MyFormId"] = myformid;
        }

        public IActionResult OnPost()
        {
            question.UpdatedAt = DateTime.Now;

            if (question.IsInUse == false)
            {
                question.OrderOfDisplay = 0;
            }
            if (ModelState.IsValid)
            {
                _db.Questions.Update(question);
                _db.SaveChanges();
                TempData["success"] = "Question has been updated.";
                return RedirectToPage("Index", new { myformid = question.MyFormId });
            }
            return Page();
        }
    }
}
