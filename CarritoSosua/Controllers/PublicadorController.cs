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
    public class PublicadorController : Controller
    {
        private CarritoSosuaContext db = new CarritoSosuaContext();

        // GET: Publicador
        public ActionResult Index()
        {
            return View(db.Publicadores.OrderBy(p => p.Name).ToList());
        }

        // GET: Publicador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publicador publicador = db.Publicadores.Find(id);
            if (publicador == null)
            {
                return HttpNotFound();
            }
            return View(publicador);
        }

        // GET: Publicador/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publicador/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Publicador publicador)
        {
            if (ModelState.IsValid)
            {
                db.Publicadores.Add(publicador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publicador);
        }

        // GET: Publicador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publicador publicador = db.Publicadores.Find(id);
            if (publicador == null)
            {
                return HttpNotFound();
            }
            return View(publicador);
        }

        // POST: Publicador/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Publicador publicador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publicador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publicador);
        }

        // GET: Publicador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publicador publicador = db.Publicadores.Find(id);
            if (publicador == null)
            {
                return HttpNotFound();
            }
            return View(publicador);
        }

        // POST: Publicador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Publicador publicador = db.Publicadores.Find(id);
            db.Publicadores.Remove(publicador);
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
