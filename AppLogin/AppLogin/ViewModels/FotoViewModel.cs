using Plugin.Media.Abstractions;
using AppLogin.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppLogin.ViewModel
{
    public class FotoViewModel : FotoModel
    {
        public Command CapturarComando { get; set; }

        public FotoViewModel()
        {
            CapturarComando = new Command(TomarFoto);
        }
        private async void TomarFoto()
        {
            var camara = new StoreCameraMediaOptions();
            camara.PhotoSize = PhotoSize.Full;
            camara.SaveToAlbum = true;
            var foto = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(camara);
            if (foto != null)
            {
                Fotico = ImageSource.FromStream(() =>
                {
                    return foto.GetStream();
                });
            }
        }
    }
}
