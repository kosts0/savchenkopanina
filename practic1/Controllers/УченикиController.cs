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
    public class УченикиController : Controller
    {
        private ps2Entities db = new ps2Entities();

        // GET: Ученики
        public ActionResult Index()
        {
            var ученики = db.Ученики.Include(у => у.Классы).Include(у => у.Родители).Where(u => u.Родители.Логин == User.Identity.Name);
            return View(ученики.ToList());
        }


        public ActionResult Progress(System.Guid id, int x)
        {
            List < Неделя_предметы_оценки > week = new List<Неделя_предметы_оценки>();
            int k = 0;
            System.DateTime date = DateTime.Now;
            while (date.DayOfWeek.ToString() != "Monday")
            {
                k++;
                date = DateTime.Now.AddDays(-1 * k + x * 7);

            }
            Guid clas = db.Ученики.Where(p => p.ID_ученика == id).FirstOrDefault().ID_класса;
            string numb = db.Классы.Where(p => p.ID_класса == clas).FirstOrDefault().Номер_класса;
            while (date.DayOfWeek.ToString() != "Sunday")
            {
                Неделя_предметы_оценки one_day = new Неделя_предметы_оценки();
                one_day.day_of_week = date.DayOfWeek.ToString();
                one_day.day = date;
                var расписаие = db.Расписание.Include(p => p.Время_уроков).Include(p => p.Предметы).Where(p => p.ID_класса == clas).Where(p => p.День_недели == date.DayOfWeek.ToString()).ToList();
                var оценки = db.Оценки.Where(p => (p.Дата.Day == date.Day && p.Дата.Month == date.Month && p.Дата.Day == date.Day)).Where(p => p.ID_ученика == id).ToList();
                
                one_day.Оценки = оценки;
                one_day.Расписание = расписаие;
                week.Add(one_day);
                k--;
                date = DateTime.Now.AddDays(-1 * k);
            }
            ViewBag.время = db.Время_уроков;
            return View(week);
        }
        // GET: Ученики/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ученики ученики = db.Ученики.Find(id);
            if (ученики == null)
            {
                return HttpNotFound();
            }
            return View(ученики);
        }

        // GET: Ученики/Create
        public ActionResult Create()
        {
            ViewBag.ID_класса = new SelectList(db.Классы, "ID_класса", "Номер_класса");
            ViewBag.ID_родителя = new SelectList(db.Родители, "ID_родителя", "Фамилия");
            return View();
        }

        // POST: Ученики/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_ученика,Фамилия,Имя,Отчество,Номер_телефона,ID_родителя,ID_класса")] Ученики ученики)
        {
            if (ModelState.IsValid)
            {
                ученики.ID_ученика = Guid.NewGuid();
                db.Ученики.Add(ученики);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_класса = new SelectList(db.Классы, "ID_класса", "Номер_класса", ученики.ID_класса);
            ViewBag.ID_родителя = new SelectList(db.Родители, "ID_родителя", "Фамилия", ученики.ID_родителя);
            return View(ученики);
        }

        // GET: Ученики/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ученики ученики = db.Ученики.Find(id);
            if (ученики == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_класса = new SelectList(db.Классы, "ID_класса", "Номер_класса", ученики.ID_класса);
            ViewBag.ID_родителя = new SelectList(db.Родители, "ID_родителя", "Фамилия", ученики.ID_родителя);
            return View(ученики);
        }

        // POST: Ученики/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_ученика,Фамилия,Имя,Отчество,Номер_телефона,ID_родителя,ID_класса")] Ученики ученики)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ученики).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_класса = new SelectList(db.Классы, "ID_класса", "Номер_класса", ученики.ID_класса);
            ViewBag.ID_родителя = new SelectList(db.Родители, "ID_родителя", "Фамилия", ученики.ID_родителя);
            return View(ученики);
        }

        // GET: Ученики/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ученики ученики = db.Ученики.Find(id);
            if (ученики == null)
            {
                return HttpNotFound();
            }
            return View(ученики);
        }

        // POST: Ученики/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Ученики ученики = db.Ученики.Find(id);
            db.Ученики.Remove(ученики);
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
