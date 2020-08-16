using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetIn.WEbApi.Models
{
    public class LogoViewModel
    {
        [StringLength(32, ErrorMessage = "Не более 32 символов")]
        public string Name { get; set; }
        [Required]
        // TODO пока текст, поменять на изображение
        [StringLength(256, ErrorMessage = "Не более 256 символов")]
        public string IconUrl { get; set; }
    }
    
    public class GetLogoResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
