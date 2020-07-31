using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetIn.Core.Entities
{
    public class Logo : BaseEntity
    {
        [StringLength(32, ErrorMessage = "Не более 32 символов")]
        public string Name { get; set; }
        [Required]
        [StringLength(256, ErrorMessage = "Не более 256 символов")]
        public string IconUrl { get; set; }
    }
}
