using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp.Models
{
    public class UserDetails
    {       
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsPremium { get; set; } = false;
        // Add other properties as needed
    }
}
