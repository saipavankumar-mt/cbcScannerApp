using cbc_testapp.Models;
using cbc_testapp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace cbc_testapp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one


            if(_userName == null || _password == null)
            {

            }
            else
            {

                var loginRequest = new LoginRequest()
                {
                    UserName = _userName,
                    Password = _password,
                    Role = "SuperAdmin"
                };

                if(App.IsDebugMode)
                {
                    var response = DataStore.CreateSession(loginRequest);
                    App.IsUserLoggedIn = true;
                    App.UserSessionId = response.SessionKey;
                    App.UserId = response.Name;

                }
                else
                {
                    var response = await webApiService.Post<LoginRequest, SessionResponse>(loginRequest, "Login");
                }

               

                await Shell.Current.GoToAsync($"//{nameof(QRScanner)}?{nameof(ScanResultViewModel.ClientName)}={App.UserId}");
            }

            
        }
    }
}
