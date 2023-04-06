using cbc_testapp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using ZXing;

namespace cbc_testapp.Services
{
     public interface ILoad
    {
        void UploadFromGallery(QRScanner editor);

        BinaryBitmap GetBinaryBitmap(byte[] image);
    }
}
