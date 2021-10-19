using Pagina_Videojuego.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using System.IO;

using System.Linq;

using System.Web;

using System.Web.Mvc;

using System.Web.Services.Description;

namespace Pagina_Videojuego.Controllers
{
	public class PeliculaController : Controller
	{
		SqlConnection cn = new SqlConnection(ConfigurationManager
												.ConnectionStrings["cnx"].ConnectionString);

		List<Producto> ListPelicula()
		{
			List<Producto> aPelicula = new List<Producto>();
			SqlCommand cmd = new SqlCommand("SP_LISTAPRODUCTO", cn)
			{
				CommandType = CommandType.StoredProcedure
			};
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				aPelicula.Add(new Producto()
				{
					Id = dr.GetString(0),
					Nombre = dr.GetString(1),
					Description = dr.GetString(2),
					TiempoEnvio = dr.GetString(3),
					FechaPub = dr.GetString(4),
					Precio = dr.GetDecimal(5),
					Stock = dr.GetInt32(6),
					Imagen = dr.GetString(7),
					Categoria = dr.GetString(8),
					Usuario = dr.GetString(9)
				});
			}
			dr.Close();
			cn.Close();
			return aPelicula;
		}

		List<PeliculaOriginal> ListPeliculaOriginal()
		{
			List<PeliculaOriginal> aPelicula = new List<PeliculaOriginal>();
			SqlCommand cmd = new SqlCommand("SP_LISTAPRODUCTSOG", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				aPelicula.Add(new PeliculaOriginal()
				{
					idPelicula = dr[0].ToString(),
					nombre = dr[1].ToString(),
					direct = dr[2].ToString(),
					duracion = dr[3].ToString(),
					fechaEstreno = dr[4].ToString(),
					precio = double.Parse(dr[5].ToString()),
					stock = int.Parse(dr[6].ToString()),
					idgenero = dr[7].ToString(),
					foto = dr[8].ToString()
				});
			}
			dr.Close();
			cn.Close();
			return aPelicula;
		}

		List<Producto> ListProductoByNombre(string nombre)
		{
			List<Producto> productos = new List<Producto>();
			SqlCommand cmd = new SqlCommand("SP_LISTA_PRODUCTO_POR_NOMBRE", cn)
			{
				CommandType = CommandType.StoredProcedure
			};
			cmd.Parameters.AddWithValue("@nombre", nombre);
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				productos.Add(new Producto()
				{
					Id = dr.GetString(0),
					Nombre = dr.GetString(1),
					Description = dr.GetString(2),
					TiempoEnvio = dr.GetString(3),
					FechaPub = dr.GetString(4),
					Precio = dr.GetDecimal(5),
					Stock = dr.GetInt32(6),
					Imagen = dr.GetString(7),
					Categoria = dr.GetString(8),
					Usuario = dr.GetString(9)
				});
			}
			dr.Close();
			cn.Close();
			return productos;
		}

		List<GeneroPelicula> listGenero()
		{
			List<GeneroPelicula> aGeneroPeli = new List<GeneroPelicula>();
			SqlCommand cmd = new SqlCommand("SP_LISTACATEGORIA", cn);
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				GeneroPelicula objC = new GeneroPelicula()
				{
					idGenero = dr[0].ToString(),
					nombre = dr[1].ToString(),
				};
				aGeneroPeli.Add(objC);
			}

			dr.Close();
			cn.Close();
			return aGeneroPeli;
		}

		string codigoCorrelativo()
		{
			string codigo = null;
			SqlCommand cmd = new SqlCommand("SP_ULTIMOCODIGOPRODUCTO", cn);
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				codigo = dr[0].ToString();
			}
			dr.Close();
			cn.Close();

			string s = codigo.Substring(4, 6);
			int s2 = int.Parse(s);
			if (s2 < 9)
			{
				s2++;
				codigo = "000000" + s2;
			}
			else if (s2 >= 9)
			{
				s2++;
				codigo = "00000" + s2;
			}
			else if (s2 >= 99)
			{
				s2++;
				codigo = "0000" + s2;
			}
			else if (s2 >= 999)
			{
				s2++;
				codigo = "000" + s2;
			}
			else if (s2 >= 9999)
			{
				s2++;
				codigo = "00" + s2;
			}
			else if (s2 >= 99999)
			{
				s2++;
				codigo = "0" + s2;
			}

			return codigo;
		}

		/*Vistas del controlador Pelicula*/

		public ActionResult Index()
		{
			return View();
		}
		public ActionResult listadoPelicula()
		{
			return View(ListPelicula());
		}

		public ActionResult listadoPeliculaPag(int p = 0)
		{
			List<Producto> aPelicula = ListPelicula();
			int filas = 10;
			int n = aPelicula.Count;
			int pag = n % filas > 0 ? n / filas + 1 : n / filas;

			ViewBag.pag = pag;
			ViewBag.p = p;
			return View(aPelicula.Skip(p * filas).Take(filas));

		}

		public ActionResult detallePelicula(string id)
		{
			Producto objP = ListPelicula().Where(p => p.Id == id).FirstOrDefault();
			return View(objP);
		}

		string CRUD(string proceso, List<SqlParameter> p)
		{
			string mensaje = "No se registro";
			cn.Open();
			try
			{
				SqlCommand cmd = new SqlCommand(proceso, cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddRange(p.ToArray());
				int n = cmd.ExecuteNonQuery();
				mensaje = n + " Registro actualizado";
			}
			catch (SqlException ex)
			{
				mensaje = ex.Message;
			}
			finally
			{
				cn.Close();
			}
			return mensaje;
		}

		public ActionResult registroPelicula()
		{
			ViewBag.codigo = codigoCorrelativo();
			ViewBag.categorias = new SelectList(listGenero(), "idGenero", "nombre");
			return View(new PeliculaOriginal());
		}

		[HttpPost]
		public ActionResult registroPelicula(PeliculaOriginal objP, HttpPostedFileBase f)
		{
			if (f == null)
			{
				ViewBag.mensaje = "Selecciona una foto";
				return View(objP);
			}
			if (Path.GetExtension(f.FileName) != ".jpg")
			{
				ViewBag.mensaje = "Debe ser un jpg";
				return View(objP);
			}
			List<SqlParameter> parameters = new List<SqlParameter>()
						{
								new SqlParameter(){ParameterName="@ID_PROD",SqlDbType=SqlDbType.Char, Value=objP.idPelicula},
								new SqlParameter(){ParameterName="@NOM_PROD",SqlDbType=SqlDbType.VarChar, Value=objP.nombre},
								new SqlParameter(){ParameterName="@DES_PROD",SqlDbType=SqlDbType.VarChar, Value=objP.direct},
								new SqlParameter(){ParameterName="@TIEMP_ENVIO",SqlDbType=SqlDbType.VarChar, Value=objP.duracion},
								new SqlParameter(){ParameterName="@FECHA_PUB",SqlDbType=SqlDbType.VarChar, Value=objP.fechaEstreno},
								new SqlParameter(){ParameterName="@PRECIO",SqlDbType=SqlDbType.SmallMoney, Value=objP.precio},
								new SqlParameter(){ParameterName="@STOCK",SqlDbType=SqlDbType.Int, Value=objP.stock},
								new SqlParameter(){ParameterName="@IM_PROD",SqlDbType=SqlDbType.Char, Value=objP.idgenero},
								new SqlParameter(){ParameterName="@ID_CATEG",SqlDbType=SqlDbType.VarChar,
														Value="~/Images/Productos/"+Path.GetFileName(f.FileName)}
						};
			ViewBag.mensaje = CRUD("SP_MANTENIMIENTOPRODUCTO", parameters);
			f.SaveAs(Path.Combine(Server.MapPath("~/Images/Productos/"),
					Path.GetFileName(f.FileName)));

			//ViewBag.genero = new SelectList(listGenero(), "ID_CATEG", "NOM_CATEG");
			return RedirectToAction("listadoPelicula");

		}

		public ActionResult editarPelicula(string id)
		{
			PeliculaOriginal peliO = ListPeliculaOriginal().Where(x => x.idPelicula == id).FirstOrDefault();

			ViewBag.genero = new SelectList(listGenero(), "ID_CATEG", "NOM_CATEG");
			return View(peliO);
		}

		[HttpPost]
		public ActionResult editarPelicula(PeliculaOriginal objP, HttpPostedFileBase f)
		{
			if (f == null)
			{
				return View(objP);
			}
			if (Path.GetExtension(f.FileName) != ".jpg")
			{
				return View(objP);
			}
			List<SqlParameter> parameters = new List<SqlParameter>()
						{
								new SqlParameter(){ParameterName="@ID_PROD",SqlDbType=SqlDbType.Char, Value=objP.idPelicula},
								new SqlParameter(){ParameterName="@NOM_PROD",SqlDbType=SqlDbType.VarChar, Value=objP.nombre},
								new SqlParameter(){ParameterName="@DES_PROD",SqlDbType=SqlDbType.VarChar, Value=objP.direct},
								new SqlParameter(){ParameterName="@TIEMP_ENVIO",SqlDbType=SqlDbType.VarChar, Value=objP.duracion},
								new SqlParameter(){ParameterName="@FECHA_PUB",SqlDbType=SqlDbType.VarChar, Value=objP.fechaEstreno},
								new SqlParameter(){ParameterName="@PRECIO",SqlDbType=SqlDbType.SmallMoney, Value=objP.precio},
								new SqlParameter(){ParameterName="@STOCK",SqlDbType=SqlDbType.Int, Value=objP.stock},
								new SqlParameter(){ParameterName="@IM_PROD",SqlDbType=SqlDbType.Char, Value=objP.idgenero},
								new SqlParameter(){ParameterName="@ID_CATEG",SqlDbType=SqlDbType.VarChar, Value="~/Images/movies/"+Path.GetFileName(f.FileName)}
						};
			ViewBag.mensaje = CRUD("SP_MANTENIMIENTOPRODUCTO", parameters);
			f.SaveAs(Path.Combine(Server.MapPath("~/Images/Productos/"),
					Path.GetFileName(f.FileName)));

			ViewBag.genero = new SelectList(listGenero(), "idGenero", "nombre", objP.idPelicula);
			return RedirectToAction("listadoPeliculaPag");
		}

		public ActionResult eliminarPelicula(string id)
		{
			Producto objP = ListPelicula().Where(x => x.Id == id).FirstOrDefault();
			List<SqlParameter> parameters = new List<SqlParameter>()
						{
								new SqlParameter(){ParameterName="@ID",SqlDbType=SqlDbType.Char, Value=objP.Id},
						};
			CRUD("SP_ELIMINARPELICULA", parameters);
			return RedirectToAction("listadoPelicula");
		}

		public ActionResult listadoPeliculaxNombre()
		{
			return View(ListProductoByNombre(""));
		}

		[HttpPost]
		public ActionResult listadoPeliculaxNombre(string nombre)
		{
			return View(ListProductoByNombre(nombre));
		}

	}
}