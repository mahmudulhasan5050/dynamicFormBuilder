using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FormWizard.Model
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public string QuestionAnswer { get; set; }
        [Required]
        public int MyFormId { get; set; }
        [ValidateNever]
        public MyForm MyForm { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [ValidateNever]
        public Question Question { get; set; }
        [Required]
        public int ParticipantId { get; set; }
        [ValidateNever]
        public Participant Participant { get; set; }
        public string? Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
