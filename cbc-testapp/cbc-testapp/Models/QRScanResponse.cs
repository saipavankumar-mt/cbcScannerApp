using System;
using System.Collections.Generic;
using System.Text;

namespace cbc_testapp.Models
{
    public class QRScanResponse
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public string Address { get; set; }
        public string Text { get; set; }

        public string QRCodeId { get; set; }
    }
}
