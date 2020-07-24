using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetIn.Core.Entities
{
    public class Logo : BaseEntity
    {
        [Required]
        [StringLength(256, ErrorMessage = "Не более 256 символов")]
        public string IconUrl { get; set; }
    }
}
