using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pagina_Videojuego.Models
{
    public class UsuarioOriginal
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese Código")]
        [DisplayName("Codigo de Usuario")]
        public string idUsuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese Nombres")]
        [DisplayName("Nombres Completos")]
        public string nombres { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese nombre del usuario")]
        [DisplayName("Usuario")]
        public string nombreUsu { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese ")]
        [DisplayName("Contraseña ")]
        public string password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "INGRESE CORREO")]
        [DisplayName("Correo")]
        public string correo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese FECHA")]
        [DisplayName("Fecha Nacimiento")]
        public DateTime fechaNaci { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese SEXO")]
        [DisplayName("Sexo")]
        public String sexo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese Pais")]
        [DisplayName("Pais")]
        public string idpais { get; set; }

        
    }
}