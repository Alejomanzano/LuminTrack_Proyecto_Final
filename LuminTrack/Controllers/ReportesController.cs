using LuminTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LuminTrack.Controllers
{
    public class ReportesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Reportes
        public ActionResult Index()
        {
            return View(db.Reportes.ToList());
        }

        private bool EsAdmin()
        {
            return Session["Rol"] != null && Session["Rol"].ToString() == "Administrador";
        }

        private bool EsCiudadano()
        {
            return Session["Rol"] != null && Session["Rol"].ToString() == "Ciudadano";
        }

        // GET: Reportes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Reporte reporte = db.Reportes.Find(id);
            if (reporte == null)
                return HttpNotFound();

            return View(reporte);
        }

        // GET: Reportes/Create
        public ActionResult Create()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("Login", "Usuarios");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reporte reporte, HttpPostedFileBase foto)
        {
            if (Session["Rol"] == null || Session["Email"] == null)
                return RedirectToAction("Login", "Usuarios");

            reporte.UsuarioEmail = Session["Email"].ToString();
            reporte.FechaCreacion = DateTime.Now;

            if (reporte.Categoria == "Otro" &&
                string.IsNullOrWhiteSpace(reporte.OtraCategoria))
            {
                ModelState.AddModelError(
                    "OtraCategoria",
                    "Debe explicar la categoría cuando selecciona 'Otro'"
                );
            }

            if (foto != null && foto.ContentLength > 0)
            {
                string carpeta = Server.MapPath("~/Uploads/Reportes/");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string nombre = Guid.NewGuid() + Path.GetExtension(foto.FileName);
                string rutaCompleta = Path.Combine(carpeta, nombre);

                foto.SaveAs(rutaCompleta);
                reporte.FotoURL = "/Uploads/Reportes/" + nombre;
            }

            if (!ModelState.IsValid)
            {
                return View(reporte);
            }

            db.Reportes.Add(reporte);
            db.SaveChanges();

            return RedirectToAction(
                Session["Rol"].ToString() == "Ciudadano"
                    ? "MisReportes"
                    : "Index"
            );
        }


        // GET: Reportes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            if (id == null)
                return HttpNotFound();

            var reporte = db.Reportes.Find(id);
            return View(reporte);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reporte reporte, HttpPostedFileBase foto)
        {
            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(reporte);

            var reporteDB = db.Reportes.Find(reporte.Id);
            if (reporteDB == null)
                return HttpNotFound();

            reporteDB.Descripcion = reporte.Descripcion;
            reporteDB.Categoria = reporte.Categoria;

            if (foto != null && foto.ContentLength > 0)
            {
                string carpeta = Server.MapPath("~/Uploads/Reportes/");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string nombre = Guid.NewGuid() + Path.GetExtension(foto.FileName);
                string ruta = Path.Combine(carpeta, nombre);
                foto.SaveAs(ruta);

                reporteDB.FotoURL = "/Uploads/Reportes/" + nombre;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Reportes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            var reporte = db.Reportes.Find(id);
            return View(reporte);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            var reporte = db.Reportes.Find(id);
            db.Reportes.Remove(reporte);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MisReportes()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Ciudadano")
                return RedirectToAction("Login", "Usuarios");

            string email = Session["Email"]?.ToString();

            var reportes = db.Reportes
                .Where(r => r.UsuarioEmail == email)
                .ToList();

            return View(reportes);
        }

        private List<string> ParroquiasQuito()
        {
            return new List<string>
    {
        "Belisario Quevedo","Carcelén","Centro Histórico","Chillogallo",
        "Chimbacalle","Cochapamba","Comité del Pueblo","Concepción",
        "Cotocollao","El Condado","El Inca","Guamaní","Iñaquito",
        "Itchimbía","Jipijapa","Kennedy","La Argelia","La Concepción",
        "La Ecuatoriana","La Ferroviaria","La Libertad","La Magdalena",
        "La Mena","Mariscal Sucre","Ponceano","Puengasí","Quitumbe",
        "Rumipamba","San Bartolo","San Isidro del Inca","San Juan",
        "Solanda","Turubamba",

        "Alangasí","Amaguaña","Atahualpa","Calacalí","Calderón",
        "Checa","Conocoto","Cumbayá","El Quinche","Gualea",
        "Guangopolo","Llano Chico","Lloa","Mindo","Nanegal",
        "Nanegalito","Nayón","Nono","Pacto","Perucho",
        "Pifo","Pintag","Pomasqui","Puéllaro","San Antonio",
        "San José de Minas","Tababela","Tumbaco","Yaruquí","Zámbiza"
    };
        }

    }
}