using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeasPrep.Models
{
    public class UserSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime LastLoginTime { get; set; }
        public bool IsPremium { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
