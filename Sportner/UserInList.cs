using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportner
{
    public class UserInList
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string City { get; set; }
        public int? PhoneNumber { get; set; }

        public override string ToString()
        {
            
            return Username + " / " + Email;
        }
    }

}
