using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.QuestionDragDrops
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


        [BindProperty(SupportsGet = true)]
        public int myFormId { get; set; }

        public async Task OnGetAsync(int myformid)
        {
            myFormId = myformid;

            if (myFormId != null)
            {
                questions = await GetQuestionsAsync();
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(string itemIds)
        {
            int counter = 1;
            List<int> itemIdList = new List<int>();
            itemIdList = itemIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            foreach (var itemId in itemIdList)
            {
                try
                {

                    Question item = _db.Questions.Where(x => x.Id == itemId).FirstOrDefault();
                    item.OrderOfDisplay = counter;

                    _db.Questions.Update(item);
                    await _db.SaveChangesAsync();
                    counter++;
                }
                catch (Exception)
                {
                    throw;
                }

            }

            return RedirectToPage("/Questions/index", new { myformid = myFormId });
        }

        private async Task<List<Question>> GetQuestionsAsync()
        {

            var query = _db.Questions.Where(u => u.MyFormId == myFormId && u.IsInUse == true).OrderBy(o => o.OrderOfDisplay);
            questionOptions = _db.QuestionOptions.Where(r => query.Any(u => u.Id == r.QuestionId)).ToList();
            ;
            return await query.ToListAsync();

        }
    }
}
