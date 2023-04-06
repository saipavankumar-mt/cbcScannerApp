using cbc_testapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cbc_testapp.Services
{
    public class MockDataStore : IDataStore
    {
        public SessionResponse CreateSession(LoginRequest request)
        {
            return new SessionResponse()
            {
                SessionKey = "b6aeec6c-6e98-4e44-a279-f45ccae7af4f",
                Name = request.UserName.Split('@')[0]
            };
        }

        public QRScanResponse GetQRScanData(string qrCodeId)
        {
            if(qrCodeId == "beaddc0c-4ed3-4631-a523-44a1599e5e16")
            {
                return new QRScanResponse()
                {
                    Name = "Pavan",
                    Id = "QYT11LP11L",
                    Address = "Texas",
                    Text = "Securtity Card",
                    QRCodeId = qrCodeId
                };
            }
            else if(qrCodeId == "9e25863b-560d-46f0-aadb-87371d09df04")
            {
                return new QRScanResponse()
                {
                    Name = "Dhananjay",
                    Id = "DJ123L114",
                    Address = "Texas",
                    Text = "Security Card",
                    QRCodeId = qrCodeId
                };
            }

            return null;
        }
    }
}