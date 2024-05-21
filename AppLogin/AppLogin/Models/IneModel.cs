using System;
using System.Collections.Generic;
using System.Text;

namespace AppLogin.Models
{
    public class IneModel
    {
        public string cUsuario { get; set; }
        public string cNumero_de_Cedula { get; set; }
        public byte[] INEF { get; set; }
        public byte[] INET { get; set; }
        public byte[] Documento { get; set; }
    }
}
