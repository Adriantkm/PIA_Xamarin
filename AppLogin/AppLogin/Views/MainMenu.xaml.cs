using AppLogin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppLogin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public List<byte[]> photosBytesList = new List<byte[]>();
        private List<ImageSource> _capturedPhotos;
        private static string _baseurl = "https://apiwebtja.somee.com/api/INE/";

        public MainMenu()
        {
            InitializeComponent();
        }

        private async Task CaptureMultiplePhotos1(int count)
        {
            _capturedPhotos = new List<ImageSource>();
            for (int i = 0; i < count; i++)
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo != null)
                {
                    var memoryStream = await photo.OpenReadAsync();
                    var imageSource = ImageSource.FromStream(() => memoryStream);
                    _capturedPhotos.Add(imageSource);
                }
            }
        }

        private async Task ValidarCbFotos()
        {
            if (cbDocumento.IsChecked)
            {
                await TomarFotos(3);
            }
            else
            {
                await TomarFotos(2);
            }
        }

        private async Task ValidarCbGaleria()
        {
            if (cbDocumento.IsChecked)
            {
                await TomarDeGaleria(3);
            }
            else
            {
                await TomarDeGaleria(2);
            }
        }

        private async void Btn_Fotos(object sender, EventArgs e)
        {
            await ValidarCbFotos();
        }

        private async void Galeria(object sender, EventArgs e)
        {
            await ValidarCbGaleria();
        }

        private async Task Guardar()
        {
            // Verificar que imgFotos contiene suficientes elementos
            if (imgsFotos.Children.Count < 2) // Cambiar el número según el contexto
            {
                Console.WriteLine("imgFotos no tiene suficientes imágenes.");
                return;
            }

            // Extraer imágenes del StackLayout y convertirlas a bytes

            byte[] IneF = { };
            byte[] IneT = { };
            byte[] Document = { };

            if (!cbDocumento.IsChecked)
            {
                IneF = photosBytesList[0];
                IneT = photosBytesList[1];
            }
            else
            {
                IneF = photosBytesList[0];
                IneT = photosBytesList[1];
                Document = photosBytesList[2];
            }

            if (IneF == null || IneT == null)
            {
                Console.WriteLine("Error: Algunos elementos no son imágenes.");
                return;
            }


            // Crear el objeto que enviarás en la solicitud POST
            var DataIne = new IneModel
            {
                cUsuario = "",
                cNumero_de_Cedula = "",
                INEF = { },
                INET = { },
                Documento = { },
            };

            var DataActivar = new ActivarModel
            {
                Variable = 0,
                cUsuario = "",
                cNumero_de_Cedula = ""
            };

            if (!cbActivar.IsChecked)
            {
                DataIne = new Models.IneModel
                {
                    cUsuario = usuario.Text,
                    cNumero_de_Cedula = "",
                    INEF = IneF,
                    INET = IneT,
                    Documento = Document,

                };

                DataActivar = new ActivarModel
                {
                    Variable = 1,
                    cUsuario = usuario.Text,
                    cNumero_de_Cedula = ""
                };

                await GuardarINE(DataIne);
                await ActivarUsuario(DataActivar);
                await DisplayAlert("Success!", "El usuario fue activado!", "Cerrar");
            }
            else
            {
                if (IsNumeric(usuario.Text) && usuario.Text.Length <= 10)
                {
                    DataIne = new Models.IneModel
                    {
                        cUsuario = "",
                        cNumero_de_Cedula = usuario.Text,
                        INEF = IneF,
                        INET = IneT,
                        Documento = Document,

                    };

                    DataActivar = new ActivarModel
                    {
                        Variable = 2,
                        cUsuario = "",
                        cNumero_de_Cedula = usuario.Text
                    };

                    await GuardarINE(DataIne);
                    await ActivarUsuario(DataActivar);
                    await DisplayAlert("Success!", "El usuario fue activado!", "Cerrar");
                }
                else
                {
                    if (!IsNumeric(usuario.Text))
                    {
                        await DisplayAlert("Atencion!", "La cedula solo puede contener caracteres numericos", "Cerrar");
                    }
                    if (usuario.Text.Length > 10)
                    {
                        await DisplayAlert("Atencion!", "La cedula solo puede contener un maximo de 10 caracteres", "Cerrar");
                    }
                }
            }
        }

        public bool IsNumeric(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            return regex.IsMatch(str);
        }

        private async Task GuardarINE(IneModel Data)
        {
            // Serializar el objeto a JSON
            string json = JsonConvert.SerializeObject(Data);

            // Crear el contenido para la solicitud POST
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Realizar la solicitud POST y esperar la respuesta
            HttpResponseMessage response = await httpClient.PostAsync(_baseurl + "GuardarINE", content);

            if (response.IsSuccessStatusCode) // Verificar si la respuesta es exitosa (código 200-299)
            {
                // Leer la respuesta como cadena
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Respuesta recibida: " + responseContent);
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        public async Task ActivarUsuario(ActivarModel Data)
        {
            // Serializar el objeto a JSON
            string json = JsonConvert.SerializeObject(Data);

            // Crear el contenido para la solicitud POST
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Realizar la solicitud POST y esperar la respuesta
            //HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
            HttpResponseMessage response = await httpClient.PutAsync(_baseurl + "ActivarUsuario", content);

            if (response.IsSuccessStatusCode) // Verificar si la respuesta es exitosa (código 200-299)
            {
                // Leer la respuesta como cadena
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Respuesta recibida: " + responseContent);
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        // Método para convertir una imagen a bytes
        public async Task<List<byte[]>> TomarFotos(int num)
        {
            //var photosBytesList = new List<byte[]>(); // Lista para almacenar las fotos como byte[]
            imgsFotos.Children.Clear();
            photosBytesList.Clear();
            // Capturar 3 fotos
            for (int i = 0; i < num; i++)
            {
                try
                {
                    var photo = await MediaPicker.CapturePhotoAsync();

                    if (photo != null)
                    {
                        // Convertir la foto a un flujo
                        using (var stream = await photo.OpenReadAsync())
                        {
                            // Convertir el flujo a byte[]
                            using (var memoryStream = new MemoryStream())
                            {
                                await stream.CopyToAsync(memoryStream);
                                var photoBytes = memoryStream.ToArray(); // Obtener byte[]
                                photosBytesList.Add(photoBytes); // Agregar a la lista
                            }
                        }
                        var imageSource = ImageSource.FromFile(photo.FullPath);
                        var image = new Image { Source = imageSource };
                        imgsFotos.Children.Add(image);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al capturar la foto {i + 1}: {ex.Message}");
                }
            }

            return photosBytesList;
        }

        public async Task<List<byte[]>> TomarDeGaleria(int num)
        {
            imgsFotos.Children.Clear();
            photosBytesList.Clear();

            for (int i = 0; i < num; i++)
            {
                var photo = await MediaPicker.PickPhotoAsync();

                if (photo != null)
                {
                    //var memoryStream = await photo.OpenReadAsync();

                    using (Stream stream = await photo.OpenReadAsync())
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            photosBytesList.Add(memoryStream.ToArray());
                            var image = new Image { Source = ImageSource.FromStream(() => memoryStream) };
                            imgsFotos.Children.Add(image);
                        }
                    }
                }
            }

            return photosBytesList;
        }

        private async void Btn_Activar(object sender, EventArgs e)
        {
            await Guardar();
        }
    }
}