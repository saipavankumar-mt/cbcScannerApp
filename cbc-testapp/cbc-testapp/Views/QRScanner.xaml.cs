using cbc_testapp.Models;
using cbc_testapp.Services;
using cbc_testapp.ViewModels;
using Java.Util.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace cbc_testapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRScanner : ContentPage
    {
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;
        ScanResultViewModel viewModelObject;
        private IDataStore DataStore => DependencyService.Get<IDataStore>();
        bool isOpenGallery = false;
        Stream imageStream = new MemoryStream();

        public QRScanner()
        {
            viewModelObject = new ScanResultViewModel();
            InitializeComponent();
            this.BindingContext = viewModelObject;
            viewModelObject.IsBusy = false;
            scannerView.IsScanning = true;
            scannerView.IsAnalyzing = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            scannerView.IsScanning = true;
            scannerView.IsAnalyzing = true;
            viewModelObject.IsBusy = false;
        }

        protected override void OnDisappearing()
        {
            scannerView.IsScanning = false;
            scannerView.IsAnalyzing = false;
            viewModelObject.IsBusy = false;
            base.OnDisappearing();
        }

        private void ToolBar_Clicked(object sender, EventArgs e)
        {
            scannerView.ToggleTorch();
            if (App.IsDebugMode)
            {
                scannerView.RaiseScanResult(new ZXing.Result("beaddc0c-4ed3-4631-a523-44a1599e5e16", null, null, ZXing.BarcodeFormat.QR_CODE));
            }
        }

        private bool LoadClientInfo(string qrCode, ScanResultViewModel _viewModelObject)
        {
            QRScanResponse clientInfo;
            if (TryGetClient(qrCode, out clientInfo))
            {
                if (string.IsNullOrEmpty(clientInfo?.Name) == false)
                {
                    _viewModelObject.QrCode = qrCode;
                    _viewModelObject.ClientName = clientInfo?.Name;
                    _viewModelObject.ClientAddress = clientInfo?.Address;
                    _viewModelObject.Text = clientInfo.Text;
                    _viewModelObject.ID = clientInfo.Id;
                    _viewModelObject.IsBusy = false;
                    _viewModelObject.ApiCallCompleted = true;
                    return true;
                }

            }
            _viewModelObject.IsBusy = false;
            // Ideally be set true here, but since it is binded to toggle button, it is triggered whenever changed
            _viewModelObject.ClientNotFound = !_viewModelObject.ClientNotFound;
            _viewModelObject.ApiCallCompleted = true;


            return false;
        }
        public bool TryGetClient(string qrCodeId, out QRScanResponse clientInfo)
        {
            clientInfo = null;
            if(App.IsDebugMode)
            {
                clientInfo = DataStore.GetQRScanData(qrCodeId);
                return true;
            }
            else
            {
                //Call API
            }
            return false;
            
        }

        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            // Stop analysis until we navigate away so we don't keep reading barcodes
            scannerView.IsAnalyzing = false;
            viewModelObject.IsBusy = true;
            Task.Run(() => LoadClientInfo(result?.Text, viewModelObject));
        }

        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModelObject.QrCode) == false)
            {
                await Application.Current.MainPage.Navigation.PopAsync(true);
            }
            else
            {
                scannerView.IsAnalyzing = true;
                viewModelObject.IsBusy = false;
            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!isOpenGallery)
            {
                isOpenGallery = true;
            }

            DependencyService.Get<ILoad>().UploadFromGallery(this);
        }

        public void SwitchView(string filePath)
        {
            imageStream = System.IO.File.OpenRead(filePath);

            var aditionalHints = new KeyValuePair<DecodeHintType, object>(key: DecodeHintType.PURE_BARCODE, value: "TRUE");
            var result = Decode(imageStream, file: "image_to_read", format: BarcodeFormat.QR_CODE, aditionalHints: new[] { aditionalHints });

            LoadClientInfo(result.Text, this.viewModelObject);

            Application.Current.MainPage.Navigation.PushAsync(new ClientDetails(this.viewModelObject), true);

        }

        private Result Decode(Stream imageStream, string file, BarcodeFormat? format = null, KeyValuePair<DecodeHintType, object>[] aditionalHints = null)
        {
            var r = GetReader(format, aditionalHints);

            MemoryStream ms = new MemoryStream();
            imageStream.CopyTo(ms);
            var bytes = ms.ToArray();

            var binaryBitmap = DependencyService.Get<ILoad>().GetBinaryBitmap(bytes);

            var result = r.decode(binaryBitmap);

            return result;
        }

        MultiFormatReader GetReader(BarcodeFormat? format, KeyValuePair<DecodeHintType, object>[] aditionalHints)
        {
            var reader = new MultiFormatReader();

            var hints = new Dictionary<DecodeHintType, object>();

            if (format.HasValue)
            {
                hints.Add(DecodeHintType.POSSIBLE_FORMATS, new List<BarcodeFormat>() { format.Value });
            }
            if (aditionalHints != null)
            {
                foreach (var ah in aditionalHints)
                {
                    hints.Add(ah.Key, ah.Value);
                }
            }

            reader.Hints = hints;

            return reader;
        }
    }
}