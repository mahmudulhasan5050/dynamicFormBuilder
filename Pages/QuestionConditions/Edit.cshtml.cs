using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.QuestionConditions
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public QuestionCondition questionCondition { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> optionList { get; set; }

        [BindProperty(SupportsGet = true)]
        public int questionId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int myFormId { get; set; }


        public void OnGet(int questionconditionid,int questionid, int myformid)
        {
            questionCondition = _db.QuestionConditions.Find(questionconditionid);
            var questionOptionFromDb = _db.QuestionOptions.Where(u => u.QuestionId == questionid);
            if (questionOptionFromDb != null)
            {
                optionList = questionOptionFromDb.Select(u =>
                   new SelectListItem
                   {
                       Text = u.OptionText,
                       Value = u.OptionText
                   }).ToList();
            }
            questionId = questionid;
            myFormId = myformid;
        }

        public IActionResult OnPost()
        {
            questionCondition.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _db.QuestionConditions.Update(questionCondition);
                _db.SaveChanges();
                TempData["success"] = "Question Condition has been updated.";
                return RedirectToPage("Index", new { questionid = questionId, myformid = myFormId });
            }
            return Page();
        }
    }
}
