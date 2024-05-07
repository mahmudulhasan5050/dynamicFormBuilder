using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace FormWizard.Pages.QuestionConditionOptions
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public QuestionConditionOption questionConditionOption { get; set; }


        [BindProperty(SupportsGet = true)]
        public int questionId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int myFormId { get; set; }

        public void OnGet(int questionconditionoptionid, int questionid, int myformid)
        {
            questionConditionOption = _db.QuestionConditionsOptions.Find(questionconditionoptionid);
            questionId = questionid;
            myFormId = myformid;
        }

        public async Task<IActionResult> OnPost()
        {
           
                _db.QuestionConditionsOptions.Remove(questionConditionOption);
                await _db.SaveChangesAsync();
                TempData["success"] = "Question Condition Option has been removed.";
                return RedirectToPage("Index", new
                {
                    questionconditionid = questionConditionOption.QuestionConditionId,
                    questionid = questionId,
                    myformid = myFormId
                });
            
            return Page();
        }
    }
}
