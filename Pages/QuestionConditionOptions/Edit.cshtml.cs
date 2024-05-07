using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace FormWizard.Pages.QuestionConditionOptions
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public QuestionConditionOption questionConditionOption { get; set; }
        [BindProperty]

        public int myFormId { get; set; }
        [BindProperty]

        public int questionId { get; set; }
        public void OnGet(int questionconditionoptionid, int questionid, int myformid)
        {
            questionConditionOption = _db.QuestionConditionsOptions.Find(questionconditionoptionid);
            ViewData["QuestionId"] = questionid;
            ViewData["MyFormId"] = myformid;

        }

        public IActionResult OnPost()
        {
            questionConditionOption.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _db.QuestionConditionsOptions.Update(questionConditionOption);
                _db.SaveChanges();
                TempData["success"] = "Question Condition Option has been updated.";
                return RedirectToPage("Index", new {questionconditionid= questionConditionOption.QuestionConditionId, questionid = questionId, myformid = myFormId });
            }
            return Page();
        }
    }
}
