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
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel loginVM;
        public LoginPage()
        {
            loginVM = new LoginViewModel();
            InitializeComponent();
            this.BindingContext = loginVM;

            UserName.Completed += UserName_Completed;
            Password.Completed += Password_Completed;
        }

        private void Password_Completed(object sender, EventArgs e)
        {
            loginVM.LoginCommand.Execute(null);
        }

        private void UserName_Completed(object sender, EventArgs e)
        {
            Password.Focus();
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

    }
}