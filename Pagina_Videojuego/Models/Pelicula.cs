using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Pagina_Videojuego.Models
{
    public class Pelicula
    {
        [DisplayName("CODIGO")]
        public string idPelicula { get; set; }

        [DisplayName("NOMBRE")]
        public string nombre { get; set; }

        [DisplayName("DIRECTOR")]
        public string direct { get; set; }

        [DisplayName("DURACION")]
        public string duracion { get; set; }

        [DisplayName("ESTRENO")]
        public string fechaEstreno { get; set; }

        [DisplayName("PRECIO")]
        public double precio { get; set; }

        [DisplayName("STOCK")]
        public int stock { get; set; }

        [DisplayName("GENERO")]
        public string genero { get; set; }

        [DisplayName("FOTO")]
        public string foto { get; set; }

//sida
    }
}