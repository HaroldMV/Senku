using Pagina_Videojuego.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

namespace Pagina_Videojuego.Controllers
{
	public class HomeController : Controller
	{

		SqlConnection cn = new SqlConnection(ConfigurationManager
												.ConnectionStrings["cnx"].ConnectionString);

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

		string codigoCorrelativo()
		{
			string codigo = null;
			SqlCommand cmd = new SqlCommand("SP_ULTIMOCODIGOUSUARIOO", cn)
			{
				CommandType = CommandType.StoredProcedure
			};

			cn.Open();

			SqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				codigo = dr[0].ToString();
			}

			dr.Close();
			cn.Close();

			string s = codigo.Substring(1, 7);
			int s2 = int.Parse(s);

			if (s2 < 9)
			{
				s2++;
				codigo = "U000000" + s2;
			}
			else if (s2 >= 9)
			{
				s2++;
				codigo = "U00000" + s2;
			}
			else if (s2 >= 99)
			{
				s2++;
				codigo = "U0000" + s2;
			}
			else if (s2 >= 999)
			{
				s2++;
				codigo = "U000" + s2;
			}
			else if (s2 >= 9999)
			{
				s2++;
				codigo = "U00" + s2;
			}
			else if (s2 >= 99999)
			{
				s2++;
				codigo = "U0" + s2;
			}
			else if (s2 >= 999999)
			{
				s2++;
				codigo = "U" + s2;
			}

			return codigo;
		}

		List<Pais> ListPais()
		{
			List<Pais> aPais = new List<Pais>();
			SqlCommand cmd = new SqlCommand("SP_LISTAPAISES", cn);
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				Pais objP = new Pais()
				{
					idPais = dr[0].ToString(),
					nombre = dr[1].ToString(),
				};
				aPais.Add(objP);
			}

			dr.Close();
			cn.Close();
			return aPais;
		}

		/*VISTAS DE HOMECONTROLLER*/

		public ActionResult Index(string message = "")
		{
			ViewBag.Message = message;
			return View();
		}

		[HttpPost]
		public ActionResult Login(string correo, string password)
		{
			if (!string.IsNullOrEmpty(correo) && !string.IsNullOrEmpty(password))
			{
				SqlCommand cmd = new SqlCommand("SP_INGRESOUSUARI0", cn)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@CORREO", correo);
				cmd.Parameters.AddWithValue("@PASSWORD", password);

				cn.Open();

				SqlDataReader dr = cmd.ExecuteReader();

				Usuario usu = null;

				while (dr.Read())
				{
					usu = new Usuario
					{
						id_usua = dr[0].ToString(),
						nombres = dr[1].ToString(),
						nombreUsu = dr[2].ToString(),
						password = dr[3].ToString(),
						correo = dr[4].ToString(),
						fechaNaci = DateTime.Parse(dr[5].ToString()),
						pais = dr[6].ToString()
					};
				}

				if (usu != null)
				{
					if (usu.id_usua == "U0000001")
					{
						FormsAuthentication.SetAuthCookie(usu.id_usua, true);
						return RedirectToAction("Index", "Profile", new { n = usu.nombres, i = usu.id_usua });
					}
					else
					{
						FormsAuthentication.SetAuthCookie(usu.id_usua, true);
						return RedirectToAction("IndexUsuario", "Profile", new { n = usu.nombres, i = usu.id_usua });
					}
				}
				else
				{
					return RedirectToAction("Index", new { message = "No se encontro usuario" });
				}
			}
			else
			{
				return RedirectToAction("Index", new { message = "Llene los campos" });
			}
		}

		[Authorize]
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index");
		}

		public ActionResult registrarUsuario()
		{
			ViewBag.codigo = codigoCorrelativo();
			ViewBag.pais = new SelectList(ListPais(), "idPais", "nombre");
			return View(new UsuarioOriginal());
		}

		[HttpPost]
		public ActionResult registrarUsuario(UsuarioOriginal obju)
		{
			List<SqlParameter> parametros = new List<SqlParameter>() {
								new SqlParameter(){ParameterName="@ID_USU",SqlDbType=SqlDbType.Char, Value=obju.idUsuario},
								new SqlParameter(){ParameterName="@NOMBRES",SqlDbType=SqlDbType.VarChar, Value=obju.nombres},
								new SqlParameter(){ParameterName="@NOM_USU",SqlDbType=SqlDbType.VarChar, Value=obju.nombreUsu},
								new SqlParameter(){ParameterName="@PASS_USU",SqlDbType=SqlDbType.VarChar, Value=obju.password},
								new SqlParameter(){ParameterName="@CORREO_USU",SqlDbType=SqlDbType.VarChar, Value=obju.correo},
								new SqlParameter(){ParameterName="@FECHA_NACI",SqlDbType=SqlDbType.Date, Value=obju.fechaNaci},
								new SqlParameter(){ParameterName="@SEXO_USU",SqlDbType=SqlDbType.VarChar, Value=obju.sexo},
								new SqlParameter(){ParameterName="@ID_PAIS",SqlDbType=SqlDbType.Char, Value=obju.idpais},

						};
			ViewBag.mensaje = CRUD("SP_NUEVOUSUARIO", parametros);
			return RedirectToAction("Index");
		}

	}
}