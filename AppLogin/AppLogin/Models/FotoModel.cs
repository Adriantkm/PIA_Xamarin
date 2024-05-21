using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace AppLogin.Model
{
    public class FotoModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string nombre = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        private ImageSource fotico;

        public ImageSource   Fotico
        {
            get { return fotico; }
            set { fotico = value;
                OnPropertyChanged();
            }
        }

    }
}
