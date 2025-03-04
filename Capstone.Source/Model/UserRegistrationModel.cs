using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models.Common.Enums;

namespace Capstone.Shared.Model
{
    public class UserRegistrationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string UserName { get; set; }

        public string Password { get; set; }
        [CompareAttribute("Password",ErrorMessage ="Passwords Doesn't Match")]
        public string ConfirmedPassword { get; set; }

        public UserTypes userTypes { get; set; }

    }
}
