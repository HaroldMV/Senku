using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senku.Models;

namespace Senku.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUser()
        {
            Usuario usuario = new Usuario();
            return View(usuario);
        }

        [HttpPost]
        public ActionResult AddUser(Usuario usuario)
        {
            return View();
        }
    }
}