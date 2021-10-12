using Pagina_Videojuego.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Pagina_Videojuego.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager
                            .ConnectionStrings["cnx"].ConnectionString);

        public static string sesion = null;


        // GET: Profile
        public ActionResult Index(string n, string id)
        {
            Usuario objU = null;

            if (sesion == null)
            {
                objU = ListUsuario().Where(p => p.id_usua == id).FirstOrDefault();
                sesion = id;
                ViewBag.nombre = n;
            }
            else if (sesion != null)
            {
                objU = ListUsuario().Where(p => p.id_usua == sesion).FirstOrDefault();
                ViewBag.nombre = sesion;
            }

            return View(objU);
        }

        public ActionResult IndexUsuario(string n, string id)
        {
            ViewBag.idUsu = id;
            ViewBag.nombre = n;
            return View();
        }

        List<Usuario> ListUsuario()
        {
            List<Usuario> aUsuario = new List<Usuario>();
            SqlCommand cmd = new SqlCommand("SP_LISTAUSUARIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aUsuario.Add(new Usuario()
                {
                    id_usua = dr[0].ToString(),
                    nombres = dr[1].ToString(),
                    nombreUsu = dr[2].ToString(),
                    password = dr[3].ToString(),
                    correo = dr[4].ToString(),
                    fechaNaci = DateTime.Parse(dr[5].ToString()),
                    sexo = dr[6].ToString(),
                    pais = dr[7].ToString()
                });
            }
            dr.Close();
            cn.Close();
            return aUsuario;
        }

    }
}