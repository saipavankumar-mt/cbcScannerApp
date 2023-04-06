using cbc_testapp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cbc_testapp.Services
{
    public interface IDataStore
    {
        SessionResponse CreateSession(LoginRequest request);

        QRScanResponse GetQRScanData(string qrCodeId);
    }
}
