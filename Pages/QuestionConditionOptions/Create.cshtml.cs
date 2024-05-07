using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.QuestionConditionOptions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public QuestionConditionOption questionConditionOption { get; set; }
        [BindProperty]
        public int questionId { get; set; }   //This is used only to pass to the URL
        [BindProperty]
        public int myFormId { get; set; }     //This is used only to pass to the URL
        public void OnGet(int questionconditionid, int questionid, int myformid)
        {
            ViewData["QuestionConditionId"] = questionconditionid;
            ViewData["QuestionId"] = questionid;
            ViewData["MyFormId"] = myformid;
            questionId = questionid;
            myFormId = myformid;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ViewData["QuestiononditionId"] = questionConditionOption.QuestionConditionId;
            }

            questionConditionOption.CreatedAt = questionConditionOption.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _db.QuestionConditionsOptions.Add(questionConditionOption);
                _db.SaveChanges();
                TempData["success"] = "Question Condition Option created successfully!";
                return RedirectToPage("Index", new { questionconditionid = questionConditionOption.QuestionConditionId, questionid = questionId, myformid = myFormId });
            }
            return Page();
        }
    }
}
