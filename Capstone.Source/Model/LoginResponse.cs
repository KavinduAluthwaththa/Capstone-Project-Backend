using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Shared.Model
{
    public class LoginResponse
    {
        public string Token {  get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string UserID { get; set; }
    }
}
