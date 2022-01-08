using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class PasswordChangeForm
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Secret { get; set; }
    }
}
