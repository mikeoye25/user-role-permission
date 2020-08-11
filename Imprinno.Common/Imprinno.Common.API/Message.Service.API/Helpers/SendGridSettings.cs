using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message.Service.API.Helpers
{
    public class SendGridSettings
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
    }
}
