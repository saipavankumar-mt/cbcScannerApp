using cbc_testapp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace cbc_testapp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}