using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetIn.WebApi.Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Пожалуйста, проверьте введенный адрес электронной почты")]
        [StringLength(32, ErrorMessage = "Адрес электронной почты не должен превышать 32 символа")]
        public string Email { get; set; }

        [StringLength(16, ErrorMessage = "Длина имени не должна превышать 16 символов")]
        public string FirstName { get; set; }

        [StringLength(32, ErrorMessage = "Длина фамилии не должна превышать 32 символа")]
        public string LastName { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Неверный формат даты")]
        public DateTimeOffset Birthday { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
