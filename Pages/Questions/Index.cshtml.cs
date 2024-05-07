using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.Questions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Question> questions { get; set; }
        [BindProperty]
        public IEnumerable<QuestionOption> questionOptions { get; set; }
        [BindProperty]
        public IEnumerable<QuestionCondition> questionConditions { get; set; }
        [BindProperty]
        public IEnumerable<QuestionConditionOption> questionConditionOptions { get; set; }


        [BindProperty]
        public bool ShowAll { get; set; }
        [BindProperty(SupportsGet = true)]
        public int myFormId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string myFormName { get; set; }

        public async Task OnGetAsync(int myformid)
        {
            myFormId = myformid;
            var formFromDb = _db.MyForms.FirstOrDefault(u => u.Id == myformid);

            if (formFromDb != null)
            {
                questions = await GetQuestionsAsync(false);
            }
        }


        public async Task<IActionResult> OnPostAsync()
        {

            questions = await GetQuestionsAsync(ShowAll);
            return Page();
        }

        private async Task<List<Question>> GetQuestionsAsync(bool showAll)
        {
            if (!ShowAll)
            {
                var query = _db.Questions.Where(u => u.MyFormId == myFormId && u.IsInUse == true).OrderBy(o=>o.OrderOfDisplay).Include(u => u.MyForm);
                questionOptions = _db.QuestionOptions.Where(r => query.Any(u => u.Id == r.QuestionId)).ToList();
                questionConditions = _db.QuestionConditions.Where(r => query.Any(u => u.Id == r.QuestionId)).ToList();
                questionConditionOptions = _db.QuestionConditionsOptions.AsEnumerable().Where(r => questionConditions.Any(u => u.Id == r.QuestionConditionId)).ToList();
                return await query.ToListAsync();
            }
            else
            {
                var query = _db.Questions.Where(u => u.MyFormId == myFormId).OrderBy(o => o.OrderOfDisplay).Include(u => u.MyForm);
                questionOptions = _db.QuestionOptions.Where(r => query.Any(u => u.Id == r.QuestionId)).ToList();
                questionConditions = _db.QuestionConditions.Where(r => query.Any(u => u.Id == r.QuestionId)).ToList();
                questionConditionOptions = _db.QuestionConditionsOptions.AsEnumerable().Where(r => questionConditions.Any(u => u.Id == r.QuestionConditionId)).ToList();
                return await query.ToListAsync();
            }
        }
    }
}
