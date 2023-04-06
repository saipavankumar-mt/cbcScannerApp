using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace cbc_testapp.ViewModels
{
    [QueryProperty(nameof(ClientName), nameof(ClientName))]
    public class ScanResultViewModel : BaseViewModel
    {

        string clientName;
        public string ClientName    
        {
            get { return clientName; }
            set { SetProperty(ref clientName, value); }
        }

        private string clientAddress;

        public string ClientAddress
        {
            get { return clientAddress; }
            set { SetProperty(ref clientAddress, value); }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }


        private string id;
        public string ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }


        private string qrCode;

        public string QrCode
        {
            get { return qrCode; }
            set { SetProperty(ref qrCode, value); }
        }

        private bool clientNotFound;

        public bool ClientNotFound
        {
            get { return clientNotFound; }
            set { SetProperty(ref clientNotFound, value); }
        }

        private bool apiCallCompleted;

        public bool ApiCallCompleted
        {
            get { return apiCallCompleted; }
            set { SetProperty(ref apiCallCompleted, value); }
        }

    }
}
