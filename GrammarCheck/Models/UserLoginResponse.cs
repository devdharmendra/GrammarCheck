using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrammarCheck.Models
{
    public class UserLoginResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string token { get; set; }
    }
}