using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Models.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneNo { get; set; }
        public string Address { get; set; }
        public UserTypes UserType { get; set; }
    }
}
