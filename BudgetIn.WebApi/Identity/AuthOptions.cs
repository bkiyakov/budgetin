using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetIn.WebApi.Identity
{
    public class AuthOptions
    {
        public const string ISSUER = "BudgetInAPI"; // издатель токена
        public const string AUDIENCE = "BudgetInClient"; // потребитель токена
        const string KEY = "budgetinsecretkey_secret16082020";   // ключ для шифрации
        public const int LIFETIME = 30; // время жизни токена - 30 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
