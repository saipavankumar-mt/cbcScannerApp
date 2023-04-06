using cbc_testapp.ViewModels;
using cbc_testapp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace cbc_testapp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(QRScanner), typeof(QRScanner));
            Routing.RegisterRoute(nameof(ClientDetails), typeof(ClientDetails));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
