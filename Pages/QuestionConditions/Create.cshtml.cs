using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.QuestionConditions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public QuestionCondition questionCondition { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> optionList { get; set; }

        [BindProperty(SupportsGet =true)]
        public int questionId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int myFormId { get; set; }

        public void OnGet(int questionid, int myformid)
        {
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
            questionCondition.CreatedAt = questionCondition.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                var myFormIdFromDb = _db.Questions.FirstOrDefault(x => x.Id == questionCondition.QuestionId);
                _db.QuestionConditions.Add(questionCondition);
                _db.SaveChanges();
                TempData["success"] = "New Question Condition has been created.";
                return RedirectToPage("Index", new { questionid = questionId, myformid = myFormId });
            }
            return Page();
        }
    }
}
