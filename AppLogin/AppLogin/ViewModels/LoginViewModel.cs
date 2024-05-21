using AppLogin.Conexion;
using AppLogin.Models;
using AppLogin.Views;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppLogin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Atributos
        private string email;
        private string password;
        #endregion

        #region Propiedades
        public string txtemail
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }
        public string txtpassword
        {
            get { return password; }
            set { SetValue(ref password, value); }
        }

        #endregion

        #region Command
        public Command LoginCommand { get; }
        #endregion

        #region Metodo
        public async Task LoginUsuario()
        {
            var objusuario = new UserModel()
            {
                EmailField = email,
                PasswordField = password,
            };
            try
            {

                var autenticacion = new FirebaseAuthProvider(new FirebaseConfig(DBConn.WebApiAuthentication));
                var authuser = await autenticacion.SignInWithEmailAndPasswordAsync(objusuario.EmailField.ToString(), objusuario.PasswordField.ToString());
                string obternertoken = authuser.FirebaseToken;

                var Propiedades_NavigationPage = new NavigationPage(new MainMenu());

                Propiedades_NavigationPage.BarBackgroundColor = Color.RoyalBlue;
                App.Current.MainPage = Propiedades_NavigationPage;


            }
            catch (Exception)
            {

                await App.Current.MainPage.DisplayAlert("Advertencia", "Los datos introducidos son incorrectos o el usuario se encuentra inactivo.", "Aceptar");
                //await App.Current.MainPage.DisplayAlert("Advertencia", ex.Message, "Aceptar");
            }
        }
        #endregion

        #region Constructor
        public LoginViewModel(INavigation navegar)
        {
            Navigation = navegar;
            LoginCommand = new Command(async () => await LoginUsuario());

        }
        #endregion

    }
}
