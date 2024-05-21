using AppLogin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppLogin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = new LoginViewModel(Navigation);

            logo.Source = ImageSource.FromFile(@"icon.png");
        }

        /*private async void Login_Clicked(object sender, EventArgs e)
        {
            string username = TxtUsuario.Text;
            string password = TxtPassword.Text;

            if (IsValidDate(username, password))
            {
                await DisplayAlert("Logrado", "Inicio exitiso", "Ok");
                await this.Navigation.PushAsync(new MainMenu());
            }
            else
            {
                await DisplayAlert("Error", "Datos incorrectos", "Ok");
            }
        }

        private bool IsValidDate(string username, string password)
        {
            return username == "1" && password == "1234";
        }*/
    }
}