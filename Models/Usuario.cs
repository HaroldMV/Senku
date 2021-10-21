using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Senku.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public string Correo { get; set; }
        public string FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string IdPais { get; set; }
        public string IdUbigeo { get; set; }
    }
}