using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetIn.Front.Models
{
    public class LoginResponseModel
    {
        public UserObject User { get; set; }
        public string Token { get; set; }
        public class UserObject
        {
            public string Username { get; set; }
            public string Role { get; set; }
        }
    }
}
