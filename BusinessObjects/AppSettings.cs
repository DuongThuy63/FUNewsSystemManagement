using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class AppSettings
    {
        public AdminAccountSettings AdminAccount { get; set; }
    }

    public class AdminAccountSettings
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
