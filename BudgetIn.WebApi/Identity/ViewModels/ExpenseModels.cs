using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetIn.WebApi.Models
{
    public class ExpenseViewModel
    {
        [Required]
        public int Sum { get; set; }
        [StringLength(128, ErrorMessage = "Максимальная длина 128 символов")]
        public string Note { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }

    public class GetExpenseResponseModel
    {
        public int ExpenseId { get; set; }
        public int Sum { get; set; }
        public string Note { get; set; }
        public int CategoryId { get; set; }
        public DateTime Date { get; set; }
    }
}
