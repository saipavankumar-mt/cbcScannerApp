using System;
using System.Collections.Generic;
using System.Text;

namespace cbc_testapp.Models
{
    public class SessionResponse
    {
        public string SessionKey { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public string GPSLocation { get; set; }

        public string Role { get; set; }
    }
}
