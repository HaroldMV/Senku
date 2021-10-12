using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pagina_Videojuego.Models
{
    public class PeliculaOriginal
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese Código")]
        [DisplayName("Codigo")]
        public string idPelicula { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese Nombre de producto")]
        [DisplayName("Nombre de producto")]
        public string nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese descripción")]
        [DisplayName("Descripción de producto")]
        public string direct { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese tiempo de envío")]
        [DisplayName("Tiempo de envío")]
        public string  duracion { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese Fecha")]
        [DisplayName("Fecha")]
        public string fechaEstreno { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese Precio")]
        [DisplayName("Precio")]
        public double precio { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese Stock")]
        [DisplayName("Stock")]
        [Range(1, int.MaxValue, ErrorMessage ="Error de Rango")]
        public int stock { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese Categoría")]
        [DisplayName("Categoria")]
        public string idgenero { get; set; }

        public string foto { get; set; }
    }
}