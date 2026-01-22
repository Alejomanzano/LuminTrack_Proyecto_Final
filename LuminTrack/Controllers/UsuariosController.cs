using LuminTrack.Migrations;
using LuminTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static System.Collections.Specialized.BitVector32;

namespace LuminTrack.Controllers
{
    public class UsuariosController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // LISTADO - LIBRE
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        // DETALLES - LIBRE
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Usuario usuario = db.Usuarios.Find(id);

            if (usuario == null)
                return HttpNotFound();

            return View(usuario);
        }

        // CREAR - LIBRE
        public ActionResult Create()
        {
            ViewBag.Roles = new SelectList(new[] { "Administrador", "Tecnico", "Ciudadano" });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario, string Password)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError("Password", "La contraseña es obligatoria");
            }
            else
            {
                // 🔥 Generar hash ANTES de validar
                usuario.PasswordHash = PasswordHelper.Hash(Password);

                // 🔥 Limpiar error previo si existía
                ModelState.Remove("PasswordHash");
            }

            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Roles = new SelectList(new[] { "Administrador", "Tecnico", "Ciudadano" }, usuario.Rol);
            return View(usuario);
        }

        // EDITAR - LIBRE
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
                return HttpNotFound();

            ViewBag.Roles = new SelectList(new[] { "Administrador", "Tecnico", "Ciudadano" }, usuario.Rol);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario, string Password)
        {
            // 🔥 Usuario original desde la BD
            var usuarioDB = db.Usuarios.Find(usuario.Id);

            if (usuarioDB == null)
                return HttpNotFound();

            // Actualizar campos editables
           
            usuarioDB.Email = usuario.Email;
            usuarioDB.Rol = usuario.Rol;

            // Si se escribió nueva contraseña
            if (!string.IsNullOrWhiteSpace(Password))
            {
                usuarioDB.PasswordHash = PasswordHelper.Hash(Password);
            }

            // 🔥 Limpiar validación de PasswordHash
            ModelState.Remove("PasswordHash");

            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Roles = new SelectList(
                new[] { "Administrador", "Tecnico", "Ciudadano" },
                usuario.Rol
            );

            return View(usuario);
        }

        // ELIMINAR - LIBRE
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Usuario usuario = db.Usuarios.Find(id);

            if (usuario == null)
                return HttpNotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Ingrese correo y contraseña";
                return View();
            }

            string hash = PasswordHelper.Hash(password);

            var user = db.Usuarios.FirstOrDefault(
                u => u.Email == email && u.PasswordHash == hash
            );

            if (user == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos";
                return View();
            }

            FormsAuthentication.SetAuthCookie(user.Email, false);
            Session["Email"] = user.Email;
            Session["Rol"] = user.Rol;

            switch (user.Rol)
            {
                case "Administrador":
                    return RedirectToAction("Index", "Admin");
                case "Tecnico":
                    return RedirectToAction("Index", "Tecnico");
                case "Ciudadano":
                    return RedirectToAction("Index", "Ciudadano");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        // LOGOUT
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool existe = db.Usuarios.Any(u => u.Email == model.Email);
            if (existe)
            {
                ModelState.AddModelError("", "El correo ya está registrado");
                return View(model);
            }

            Usuario nuevo = new Usuario
            {
                Email = model.Email,
                PasswordHash = PasswordHelper.Hash(model.Password),
                Rol = "Ciudadano"
            };

            db.Usuarios.Add(nuevo);
            db.SaveChanges();

            return RedirectToAction("Login");
        }


        public ActionResult OlvidoPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OlvidoPassword(string email)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                ViewBag.Mensaje = "No existe una cuenta con ese correo.";
                return View();
            }

            return RedirectToAction("ResetPassword", new { email = email });
        }

        public ActionResult ResetPassword(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string email, string nuevaPassword)
        {
            var user = db.Usuarios.FirstOrDefault(u => u.Email == email);
            user.PasswordHash = PasswordHelper.Hash(nuevaPassword);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

    }
}