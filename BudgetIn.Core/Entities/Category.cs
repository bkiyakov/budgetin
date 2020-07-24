using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetIn.Core.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(32, ErrorMessage = "Не более 32 символов")]
        public string Name { get; set; }
        [Required]
        public int LogoId { get; set; }
        [ForeignKey("LogoId")]
        public virtual Logo Logo { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
