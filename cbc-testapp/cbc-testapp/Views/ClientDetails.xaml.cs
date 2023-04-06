using cbc_testapp.Models;
using cbc_testapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cbc_testapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientDetails : ContentPage
    {
        public ClientDetails(ScanResultViewModel viewObject)
        {
            InitializeComponent();
            OnPageLoad(viewObject);
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            //Navigation.InsertPageBefore(new LoginPage("User Logged Out"), Navigation.NavigationStack.First());
            await Navigation.PopToRootAsync();
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(QRScanner)}?{nameof(ScanResultViewModel.ClientName)}={App.UserId}");
        }

        private async void OnPageLoad(ScanResultViewModel viewObject)
        {
            lblClientId.Text = viewObject?.ID;
            lblName.Text = viewObject?.ClientName;
            lblAddress.Text = viewObject?.ClientAddress;
            lblQRCode.Text = viewObject?.QrCode;

        }


        public bool TryGetClient(string qrCodeId, out QRScanResponse clientInfo)
        {
            clientInfo = null;
            if (App.IsDebugMode)
            {
                clientInfo = new QRScanResponse()
                {
                    Name = "Pavan",
                    Id = "QYT11LP11L",
                    Address = "Texas",
                    Text = "Securtity Card",
                    QRCodeId = qrCodeId
                };

                return true;
            }
            else
            {
                //Call API
            }
            return false;

        }
    }
}