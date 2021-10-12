using System;
using System.ComponentModel;

namespace Pagina_Videojuego.Models
{
    public class Usuario
    {
        [DisplayName("CODIGO")]
        public string id_usua { get; set; }

        [DisplayName("NOMBRES")]
        public string nombres { get; set; }

        [DisplayName("NICKNAME")]
        public string nombreUsu { get; set; }

        [DisplayName("PASSWORD")]
        public string password { get; set; }

        [DisplayName("CORREO")]
        public string correo { get; set; }

        [DisplayName("FECHA NACIMIENTO")]
        public DateTime fechaNaci { get; set; }

        [DisplayName("SEXO")]
        public string sexo { get; set; }

        [DisplayName("PAIS")]
        public string pais{ get; set; }
    }
}