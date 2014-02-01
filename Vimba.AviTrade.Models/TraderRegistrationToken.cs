using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    // The registration token is used to track trader users registrations.To make it unique, it is composed of the trader id followed 
    // by a hiphen and a random 8-digit string.
    // Once used, the system will automatically create a new one and send it over to the trader so he/she can re-use it.
    public class TraderRegistrationToken
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Token { get; set; }
        public Trader Trader { get; set; }
        public Role Role { get; set; }
    }
}
