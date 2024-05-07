using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FormWizard.Model
{
    public class QuestionConditionOption
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Option Text")]
        public string ConditionOptionText { get; set; }
        [Display(Name = "Set a value for this option")]
        public string OptionValue { get; set; } = string.Empty;
        [Display(Name = "Order of Display as option.")]
        public int? OrderOfDisplay { get; set; }
        [ValidateNever]
        public QuestionCondition QuestionCondition { get; set; }
        [Required]
        public int QuestionConditionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
