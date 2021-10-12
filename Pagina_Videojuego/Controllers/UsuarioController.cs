using System.Web.Mvc;
using Pagina_Videojuego.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace Pagina_Videojuego.Controllers
{
    public class UsuarioController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager
                             .ConnectionStrings["cnx"].ConnectionString);
        

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
                    pais = dr[7].ToString(),
                });
            }
            dr.Close();
            cn.Close();
            return aUsuario;
        }

        List<UsuarioOriginal> ListUsuarioOriginal()
        {
            List<UsuarioOriginal> aaUsuario = new List<UsuarioOriginal>();
            SqlCommand cmd = new SqlCommand("SP_LISTAUSUARIOORIGINAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aaUsuario.Add(new UsuarioOriginal()
                {
                    idUsuario = dr[0].ToString(),
                    nombres = dr[1].ToString(),
                    nombreUsu = dr[2].ToString(),
                    password = dr[3].ToString(),
                    correo = dr[4].ToString(),
                    fechaNaci = DateTime.Parse(dr[5].ToString()),
                    sexo = dr[6].ToString(),
                    idpais = dr[7].ToString(),
                });
            }
            dr.Close();
            cn.Close();
            return aaUsuario;
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

        List<Usuario> ListUsuarioxNombre(string nombre)
        {
            List<Usuario> aUsuario = new List<Usuario>();
            SqlCommand cmd = new SqlCommand("SP_LISTAUSUARIOXNOMBRE", cn);
            cmd.Parameters.AddWithValue("@NOM", nombre);
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
                    pais = dr[7].ToString(),
                });
            }
            dr.Close();
            cn.Close();
            return aUsuario;
        }

        string codigoCorrelativo()
        {
            string codigo = null;
            SqlCommand cmd = new SqlCommand("SP_ULTIMOCODIGOUSUARIO", cn);
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

        /*VISTAS DEL CONTROLADOR*/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult listadoUsuario()
        {
            return View(ListUsuario());
        }

        public ActionResult listadoUsuarioPag(int p = 0)
        {
            List<Usuario> aUsuario = ListUsuario();
            int filas = 10;
            int n = aUsuario.Count;
            int pag = n % filas > 0 ? n / filas + 1 : n / filas;

            ViewBag.pag = pag;
            ViewBag.p = p;
            return View(aUsuario.Skip(p * filas).Take(filas));

        }

        public ActionResult detalleUsuario(string id)
        {
            Usuario objU = ListUsuario().Where(p => p.id_usua == id).FirstOrDefault();
            return View(objU);
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

        public ActionResult listadoUsuarioxNombre()
        {
            return View(ListUsuarioxNombre(""));
        }

        [HttpPost]
        public ActionResult listadoUsuarioxNombre(string nombre)
        {
            return View(ListUsuarioxNombre(nombre));
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
            ViewBag.mensaje = CRUD("SP_MANTENIMIENTOUSUARIO", parametros);
            return RedirectToAction("listadoUsuario");
        }

        public ActionResult editarUsuario(string id)
        {
            UsuarioOriginal usuaO = ListUsuarioOriginal().Where(x => x.idUsuario == id).FirstOrDefault();

            ViewBag.pais = new SelectList(ListPais(), "idPais", "nombre");
            return View(usuaO);
        }

        [HttpPost]
        public ActionResult editarPelicula(UsuarioOriginal objU)
        { 
            List<SqlParameter> parametros = new List<SqlParameter>() {
                new SqlParameter(){ParameterName="@ID_USU",SqlDbType=SqlDbType.Char, Value=objU.idUsuario},
                new SqlParameter(){ParameterName="@NOMBRES",SqlDbType=SqlDbType.VarChar, Value=objU.nombres},
                new SqlParameter(){ParameterName="@NOM_USU",SqlDbType=SqlDbType.VarChar, Value=objU.nombreUsu},
                new SqlParameter(){ParameterName="@PASS_USU",SqlDbType=SqlDbType.VarChar, Value=objU.password},
                new SqlParameter(){ParameterName="@CORREO_USU",SqlDbType=SqlDbType.VarChar, Value=objU.correo},
                new SqlParameter(){ParameterName="@FECHA_NACI",SqlDbType=SqlDbType.Date, Value=objU.fechaNaci},
                new SqlParameter(){ParameterName="@SEXO_USU",SqlDbType=SqlDbType.VarChar, Value=objU.sexo},
                new SqlParameter(){ParameterName="@ID_PAIS",SqlDbType=SqlDbType.Char, Value=objU.idpais},

            };
            ViewBag.mensaje = CRUD("SP_MANTENIMIENTOUSUARIO", parametros);
            ViewBag.pais = new SelectList(ListPais(), "idPais", "nombre");
            return RedirectToAction("listadoPelicula");
        }

        public ActionResult eliminarUsuario(string id)
        {
            Usuario objU = ListUsuario().Where(x => x.id_usua == id).FirstOrDefault();
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter(){ParameterName="@ID",SqlDbType=SqlDbType.Char, Value=objU.id_usua}
            };
            CRUD("SP_ELIMINARUSUARIO", parameters);
            return RedirectToAction("listadoPelicula");
        }

    }
}




        
