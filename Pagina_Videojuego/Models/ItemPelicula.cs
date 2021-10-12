using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Pagina_Videojuego.Models
{
    public class ItemPelicula
    {
        [DisplayName("CODIGO")]
        public string codigo { get; set; }

        [DisplayName("NOMBRE")]
        public string nombre { get; set; }

        [DisplayName("PRECIO")]
        public double precio { get; set; }

        [DisplayName("CANTIDAD")]
        public int cantidad { get; set; }

        [DisplayName("SUBTOTAL")]
        public double subtotal {
            get { return cantidad * precio; }
        }

        [DisplayName("FOTO")]
        public string foto { get; set; }
    }
}