using LuminTrack.Migrations;
using LuminTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LuminTrack.Controllers
{
    public class OrdenesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View(db.OrdenesTrabajo.ToList());
        }
        private bool EsAdmin()
        {
            return Session["Rol"] != null && Session["Rol"].ToString() == "Administrador";
        }

        private bool EsTecnico()
        {
            return Session["Rol"] != null && Session["Rol"].ToString() == "Tecnico";
        }

        [HttpPost]
        


        // GET: Ordenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            OrdenTrabajo orden = db.OrdenesTrabajo.Find(id);

            if (orden == null)
                return HttpNotFound();

            return View(orden);
        }

        // GET: Ordenes/Create
        public ActionResult Create()
        {
            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            ViewBag.Tecnicos = new SelectList(
                db.Usuarios.Where(u => u.Rol == "Tecnico"),
                "Email",
                "Email"
            );

            ViewBag.Reportes = new SelectList(db.Reportes, "Id", "Descripcion");
            ViewBag.Luminarias = new SelectList(db.Luminarias, "Id", "Tipo");

            return View();
        }

        // POST: Ordenes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrdenTrabajo orden, HttpPostedFileBase evidencia)
        {
            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            if (evidencia != null && evidencia.ContentLength > 0)
            {
                string carpeta = Server.MapPath("~/Uploads");
                if (!System.IO.Directory.Exists(carpeta))
                    System.IO.Directory.CreateDirectory(carpeta);

                string nombre = Guid.NewGuid() + System.IO.Path.GetExtension(evidencia.FileName);
                string ruta = "/Uploads/" + nombre;
                evidencia.SaveAs(Server.MapPath("~" + ruta));
                orden.FotoEvidenciaURL = ruta;
            }

            if (ModelState.IsValid)
            {
                db.OrdenesTrabajo.Add(orden);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // 🔥 ESTO ES LO QUE FALTABA 🔥
            ViewBag.Tecnicos = new SelectList(
                db.Usuarios.Where(u => u.Rol == "Tecnico"),
                "Email",
                "Email",
                orden.TecnicoEmail
            );

            ViewBag.Reportes = new SelectList(
                db.Reportes,
                "Id",
                "Descripcion",
                orden.ReporteId
            );

            ViewBag.Luminarias = new SelectList(
                db.Luminarias,
                "Id",
                "Tipo",
                orden.LuminariaId
            );

            return View(orden);
        }

        // GET: Ordenes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orden = db.OrdenesTrabajo.Find(id);
            if (orden == null)
                return HttpNotFound();

            // 🔹 Llenar dropdowns antes de pasar a la vista
            ViewBag.Tecnicos = new SelectList(
                db.Usuarios.Where(u => u.Rol == "Tecnico"),
                "Email",
                "Email",
                orden.TecnicoEmail
            );

            ViewBag.Reportes = new SelectList(
                db.Reportes,
                "Id",
                "Descripcion",
                orden.ReporteId
            );

            ViewBag.Luminarias = new SelectList(
                db.Luminarias,
                "Id",
                "Tipo",
                orden.LuminariaId
            );

            return View(orden);
        }


        // POST: Ordenes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrdenTrabajo orden, HttpPostedFileBase evidencia)
        {
            if (evidencia != null && evidencia.ContentLength > 0)
            {
                string ruta = "/Uploads/" + evidencia.FileName;
                evidencia.SaveAs(Server.MapPath("~" + ruta));
                orden.FotoEvidenciaURL = ruta;
            }

            if (ModelState.IsValid)
            {
                db.Entry(orden).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Reportes = new SelectList(db.Reportes, "Id", "Descripcion", orden.ReporteId);
            ViewBag.Luminarias = new SelectList(db.Luminarias, "Id", "Tipo", orden.LuminariaId);

            return View(orden);
        }

        // GET: Ordenes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            var orden = db.OrdenesTrabajo.Find(id);
            return View(orden);
        }

        // POST: Ordenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrdenTrabajo orden = db.OrdenesTrabajo.Find(id);
            db.OrdenesTrabajo.Remove(orden);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MisOrdenes()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Tecnico")
                return RedirectToAction("Login", "Usuarios");

            string emailTecnico = Session["Email"].ToString();

            var ordenes = db.OrdenesTrabajo
                .Where(o => o.TecnicoEmail == emailTecnico)
                .ToList();

            return View(ordenes);
        }

        public ActionResult Actualizar(int id)
        {
            if (!EsTecnico())
                return RedirectToAction("Index", "Home");

            var orden = db.OrdenesTrabajo.Find(id);
            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Actualizar(int id, string Estado, HttpPostedFileBase evidencia)
        {
            if (!EsTecnico())
                return RedirectToAction("Index", "Home");

            var orden = db.OrdenesTrabajo.Find(id);

            orden.Estado = Estado;

            if (evidencia != null && evidencia.ContentLength > 0)
            {
                string ruta = "/Uploads/" + evidencia.FileName;
                evidencia.SaveAs(Server.MapPath("~" + ruta));
                orden.FotoEvidenciaURL = ruta;
            }

            db.SaveChanges();
            return RedirectToAction("MisOrdenes");
        }
    }
}