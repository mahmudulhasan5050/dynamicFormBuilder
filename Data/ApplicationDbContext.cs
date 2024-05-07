using FormWizard.Model;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Data
{
    public class ApplicationDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<MyForm> MyForms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<QuestionCondition> QuestionConditions { get; set; }
        public DbSet<QuestionConditionOption> QuestionConditionsOptions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerCondition> AnswerConditions { get; set; }
        public DbSet<Participant> Participants { get; set; }


    }
}
