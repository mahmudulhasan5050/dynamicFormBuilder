using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.Questions
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;


        public DeleteModel(ApplicationDbContext db)
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

        public async Task<IActionResult> OnPost()
        {
            var questionToDelete = _db.Questions.Find(question.Id); 
            var questionOptionsFromDb = _db.QuestionOptions.Where(u => u.QuestionId == question.Id);
            
            if (questionOptionsFromDb == null)
            {
                NotFound();
            }
            if (questionOptionsFromDb != null)
            {
              
                _db.QuestionOptions.RemoveRange(questionOptionsFromDb);
                await _db.SaveChangesAsync();
            }

            if (questionToDelete != null)
            {
                _db.Questions.Remove(questionToDelete);
                await _db.SaveChangesAsync();
                TempData["success"] = "Question has been deleted.";
                return RedirectToPage("Index", new { myformid = questionToDelete.MyFormId });
            }

            return Page();
        }

    }
}
