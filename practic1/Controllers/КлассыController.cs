using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using practic1.Models;

namespace practic1.Controllers
{
    public class КлассыController : Controller
    {
        private ps2Entities db = new ps2Entities();

        // GET: Классы
        [Authorize(Roles = "Учитель")]
        public ActionResult Index()
        {

            var id = db.Учителя.FirstOrDefault(u => u.Логин == this.User.Identity.Name);
            
            var классы = db.Классы.Include(к => к.Учителя).Where(u => u.ID_классного_руководителя == id.ID_учителя);
            return View(классы.ToList());
        }

        // GET: Классы/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Классы классы = db.Классы.Find(id);
            if (классы == null)
            {
                return HttpNotFound();
            }
            return View(классы);
        }

        // GET: Классы/Create
        public ActionResult Create()
        {
            ViewBag.ID_классного_руководителя = new SelectList(db.Учителя, "ID_учителя", "Фамилия");
            return View();
        }

        // POST: Классы/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_класса,Номер_класса,Год_начала_обучения,Год_окончания_обучения,Специализация_класса,ID_классного_руководителя")] Классы классы)
        {
            if (ModelState.IsValid)
            {
                классы.ID_класса = Guid.NewGuid();
                db.Классы.Add(классы);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_классного_руководителя = new SelectList(db.Учителя, "ID_учителя", "Фамилия", классы.ID_классного_руководителя);
            return View(классы);
        }

        // GET: Классы/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Классы классы = db.Классы.Find(id);
            if (классы == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_классного_руководителя = new SelectList(db.Учителя, "ID_учителя", "Фамилия", классы.ID_классного_руководителя);
            return View(классы);
        }

        // POST: Классы/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_класса,Номер_класса,Год_начала_обучения,Год_окончания_обучения,Специализация_класса,ID_классного_руководителя")] Классы классы)
        {
            if (ModelState.IsValid)
            {
                db.Entry(классы).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_классного_руководителя = new SelectList(db.Учителя, "ID_учителя", "Фамилия", классы.ID_классного_руководителя);
            return View(классы);
        }

        // GET: Классы/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Классы классы = db.Классы.Find(id);
            if (классы == null)
            {
                return HttpNotFound();
            }
            return View(классы);
        }

        // POST: Классы/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Классы классы = db.Классы.Find(id);
            db.Классы.Remove(классы);
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
