using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetIn.Core.Entities
{
    public class Expense : BaseEntity
    {
        [Required]
        public int Sum { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [StringLength(128, ErrorMessage = "Не более 128 символов")]
        public string Note { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
