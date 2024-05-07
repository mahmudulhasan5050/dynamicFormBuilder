using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FormWizard.Model
{
    public class QuestionCondition
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string QuestionText { get; set; }
        public string? QuestionDescription { get; set; }
        public enum QuestionType
        {
            [Display(Name = "Plain Text Answer")]
            text,
            [Display(Name = "One Answer from Different Options")]
            radio,
            [Display(Name = "Multiple Answers from Different Options")]
            checkbox,
            [Display(Name = "Number as Answer")]
            number,
            [Display(Name = "Date as Answer")]
            date,
            [Display(Name = "One Answer from List Item")]
            list
        }
        [Required]
        public QuestionType Type { get; set; }
        [Required]
        [Display(Name ="Required")]
        public bool IsRequired { get; set; } = false;
        public int? OrderOfDisplay { get; set; }
        [Display(Name ="Choose Option")]
        public string? Value { get; set; }
        [ValidateNever]
        public Question Question { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
