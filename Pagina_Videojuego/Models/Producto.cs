using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pagina_Videojuego.Models
{
	public class Producto
	{
		public string Id { get; set; }
		public string Nombre { get; set; }
		public string Description { get; set; }
		public string TiempoEnvio { get; set; }
		public string FechaPub { get; set; }
		public decimal Precio { get; set; }
		public int Stock { get; set; }
		public string Imagen { get; set; }
		public string Categoria { get; set; }
		public string Usuario { get; set; }
	}
}