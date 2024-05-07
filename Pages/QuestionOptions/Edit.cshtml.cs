using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.QuestionOptions
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public QuestionOption questionOption { get; set; }
        public void OnGet(int questionoptionid, int myformid)
        {
            questionOption = _db.QuestionOptions.Find(questionoptionid);
            ViewData["MyFormId"] = myformid;
        }

        public IActionResult OnPost()
        {
            questionOption.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                var formInfoFromDb = _db.Questions.FirstOrDefault(x => x.Id == questionOption.QuestionId);
                _db.QuestionOptions.Update(questionOption);
                _db.SaveChanges();
                TempData["success"] = "Question Option has been updated.";
                return RedirectToPage("Index", new { questionid = questionOption.QuestionId, questiontype = questionOption.OptionType, myformid = formInfoFromDb.MyFormId });
            }
            return Page();
        }
    }
}
