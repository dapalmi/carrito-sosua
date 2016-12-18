using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarritoSosua.DAL;
using CarritoSosua.Models;

namespace CarritoSosua.Controllers
{
    [Authorize(Users = "dapalmi@t-online.de, davidguidetti@gmail.com, alvarado_0079@hotmail.com, lfharder@web.de, brian14martinez@icloud.com")]
    public class TurnoController : Controller
    {
        private CarritoSosuaContext db = new CarritoSosuaContext();

        // GET: Turno
        public ActionResult Index()
        {
            var turnos = db.Turnos.OrderBy(t => t.Localidad.Name).ThenBy(t => t.DayOfWeek).ThenBy(t => t.TimeFrom.Hour).ThenBy(t => t.TimeFrom.Minute).ThenBy(t => t.TimeFrom.Second).Include(t => t.Localidad);
            return View(turnos.ToList());
        }

        // GET: Turno/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = db.Turnos.Find(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            return View(turno);
        }

        // GET: Turno/Create
        public ActionResult Create()
        {
            ViewBag.LocalidadId = new SelectList(db.Localidades, "Id", "Name");
            return View();
        }

        // POST: Turno/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DayOfWeek,TimeFrom,TimeTo,LocalidadId")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Turnos.Add(turno);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocalidadId = new SelectList(db.Localidades, "Id", "Name", turno.LocalidadId);
            return View(turno);
        }

        // GET: Turno/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = db.Turnos.Find(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocalidadId = new SelectList(db.Localidades, "Id", "Name", turno.LocalidadId);
            return View(turno);
        }

        // POST: Turno/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DayOfWeek,TimeFrom,TimeTo,LocalidadId")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocalidadId = new SelectList(db.Localidades, "Id", "Name", turno.LocalidadId);
            return View(turno);
        }

        // GET: Turno/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = db.Turnos.Find(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            return View(turno);
        }

        // POST: Turno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Turno turno = db.Turnos.Find(id);
            db.Turnos.Remove(turno);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
