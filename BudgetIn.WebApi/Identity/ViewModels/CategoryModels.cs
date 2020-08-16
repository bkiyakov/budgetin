using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetIn.WebApi.Models
{
    public class CategoryViewModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "Не более 32 символов")]
        public string Name { get; set; }
        [Required]
        public int LogoId { get; set; }
    }

    public class GetCategoryResponseModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int LogoId { get; set; }
    }
}
