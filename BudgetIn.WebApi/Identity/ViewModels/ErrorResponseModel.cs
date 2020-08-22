using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetIn.WebApi.Identity.ViewModels
{
    public class ErrorResponseModel
    {
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Error(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
