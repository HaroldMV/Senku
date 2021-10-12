using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Pagina_Videojuego.Models;

namespace Pagina_Videojuego.Controllers
{
    public class Tienda_peliculaController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.
            ConnectionStrings["cnx"].ConnectionString);

        List<Pelicula> ListPelicula()
        {
            List<Pelicula> aPelicula = new List<Pelicula>();
            SqlCommand cmd = new SqlCommand("SP_LISTAPRODUCTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    aPelicula.Add(new Pelicula()
                    {
                        idPelicula = dr[0].ToString(),
                        nombre = dr[1].ToString(),
                        direct = dr[2].ToString(),
                        duracion = dr[3].ToString(),
                        fechaEstreno = dr[4].ToString(),
                        precio = double.Parse(dr[5].ToString()),
                        stock = int.Parse(dr[6].ToString()),
                        genero = dr[7].ToString(),
                        foto = dr[8].ToString()
                    });
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception)
            {

            }
            
            
            return aPelicula;
        }



        // GET: Tienda_pelicula
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult carritoCompras()
        {
            if (Session["carrito"] == null)
            {
                Session["carrito"] = new List<ItemPelicula>();
            }
            return View(ListPelicula());
        }

        public ActionResult seleccionaPelicula(string id)
        {
            Pelicula objP = ListPelicula().Where(p => p.idPelicula == id).FirstOrDefault();
            return View(objP);
        }

        public ActionResult agregarPelicula(string id, int cant=0)
        {
            var miPelicula = ListPelicula().Where(p => p.idPelicula == id).FirstOrDefault();

            ItemPelicula objP = new ItemPelicula()
            {
                codigo = miPelicula.idPelicula,
                nombre = miPelicula.nombre,
                precio = miPelicula.precio,
                cantidad = cant,
                foto = miPelicula.foto
            };

            var miCarrito = (List<ItemPelicula>)Session["carrito"];
            miCarrito.Add(objP);
            Session["carrito"] = miCarrito;

            return RedirectToAction("carritoCompras");
        }

        public ActionResult comprar()
        {
            if (Session["carrito"] == null)
            {
                return RedirectToAction("carritoCompras");
            }

            var miCarrito = (List<ItemPelicula>)Session["carrito"];
            ViewBag.total = miCarrito.Sum(s => s.subtotal);

            return View(miCarrito);
        }

        public ActionResult eliminarItem(string id = null)
        {
            if (id == null)
                return RedirectToAction("carritoCompras");

            var miCarrito = (List<ItemPelicula>)Session["carrito"];
            var item = miCarrito.Where(i => i.codigo == id).FirstOrDefault();
            miCarrito.Remove(item);

            Session["carrito"] = miCarrito;

            return RedirectToAction("comprar");
        }

    }
}