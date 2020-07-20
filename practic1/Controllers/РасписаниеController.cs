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
    public class РасписаниеController : Controller
    {
        private ps2Entities db = new ps2Entities();

        // GET: Расписание

        public bool Check(Guid clas,Guid predmet, string day)
        {
            var расписание = db.Расписание.Where(p => p.ID_предмета == predmet).Where(p => p.Классы.ID_класса == clas).Where(p => p.День_недели == day).ToList();
            if(расписание == null || расписание.Count == 0)
            {
                return false;
            }

            return true;
        }
        public ActionResult Index(Guid clas,string this_day,int x)
        {
            List<Chesk> неделя = new List<Chesk>();
            System.DateTime date = DateTime.Now;
            int k = 0;
            while (date.DayOfWeek.ToString() != "Monday")
            {
                k++;
                date = DateTime.Now.AddDays(-1 * k + x * 7);

            }

            string numb = db.Классы.Where(p => p.ID_класса == clas).FirstOrDefault().Номер_класса;
            while (date.DayOfWeek.ToString() != "Sunday")
            {
                Chesk dat = new Chesk();
                dat.День_недели = date.DayOfWeek.ToString();
                dat.ID_класса = clas;
                dat.Номер_класса = numb;
                var предметы = db.Расписание.Include(p => p.Предметы).Include(p => p.Время_уроков).Where(p => p.ID_класса == clas).Where(p => p.День_недели == date.DayOfWeek.ToString()).ToList();
                dat.Предметы = предметы;
                неделя.Add(dat);
                k--;
                date = DateTime.Now.AddDays(-1 * k);
            }
            ViewBag.время = db.Время_уроков;
            return View(неделя);
        }


        public ActionResult Index2(Guid clas,int x, Guid predmet)
        {
            Неделя_оценкиcs week = new Неделя_оценкиcs();
            System.DateTime date = DateTime.Now.AddDays(x*7);
            int k = 0;
            while(date.DayOfWeek.ToString() != "Monday")
            {
                k++;
                date = DateTime.Now.AddDays(-1*k + x*7);

            }

            DateTime l = date;
            while(date.DayOfWeek.ToString() != "Sunday")
            {
                if (Check(clas, predmet, date.DayOfWeek.ToString()) == true)
                {
                    week.Days.Add(date);
                }
                k--;
                date = DateTime.Now.AddDays(-1 * k + x*7);
            }
            week.название_предмета = db.Предметы.Where(p => p.ID_предмета == predmet).FirstOrDefault().Название_предмета;
            DateTime r = date;
            week.ID_класса = clas;
            week.номер_класса = db.Классы.Where(p => p.ID_класса == clas).FirstOrDefault().Номер_класса;
            week.предмет = predmet;
            var оценки = db.Оценки.Where(p => p.id_предмета == predmet).Where(p => p.Ученики.ID_класса == clas);
            week.Ученики = db.Ученики.Where(p => p.ID_класса == clas).ToList();
            foreach(var item in оценки)
            {
                if(item.Дата.Day >=l.Day && item.Дата.Month >= l.Month && item.Дата.Year >= l.Year && item.Дата.Year <= r.Year && item.Дата.Month <= r.Month && item.Дата.Day <= r.Day)
                {
                    week.Оценки.Add(item);
                }
            }
            week.x = x;
            return View(week);
        }

        public ActionResult In_mark(Guid clas, Guid person, DateTime day, Guid predmet, int x)
        {
            return RedirectToAction("Create", "Оценки", new { класс = clas, ученик = person, день = day, предмет = predmet, back = x });
        }
        // GET: Расписание/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Расписание расписание = db.Расписание.Find(id);
            if (расписание == null)
            {
                return HttpNotFound();
            }
            return View(расписание);
        }

        // GET: Расписание/Create
        public ActionResult Create()
        {
            ViewBag.Номер_урока = new SelectList(db.Время_уроков, "Номер_урока", "Номер_урока");
            ViewBag.ID_класса = new SelectList(db.Классы, "ID_класса", "Номер_класса");
            ViewBag.ID_предмета = new SelectList(db.Предметы, "ID_предмета", "Название_предмета");
            return View();
        }

        // POST: Расписание/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "День_недели,Номер_урока,ID_предмета,ID_класса")] Расписание расписание)
        {
            if (ModelState.IsValid)
            {
                db.Расписание.Add(расписание);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Номер_урока = new SelectList(db.Время_уроков, "Номер_урока", "Номер_урока", расписание.Номер_урока);
            ViewBag.ID_класса = new SelectList(db.Классы, "ID_класса", "Номер_класса", расписание.ID_класса);
            ViewBag.ID_предмета = new SelectList(db.Предметы, "ID_предмета", "Название_предмета", расписание.ID_предмета);
            return View(расписание);
        }

        // GET: Расписание/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Расписание расписание = db.Расписание.Find(id);
            if (расписание == null)
            {
                return HttpNotFound();
            }
            ViewBag.Номер_урока = new SelectList(db.Время_уроков, "Номер_урока", "Номер_урока", расписание.Номер_урока);
            ViewBag.ID_класса = new SelectList(db.Классы, "ID_класса", "Номер_класса", расписание.ID_класса);
            ViewBag.ID_предмета = new SelectList(db.Предметы, "ID_предмета", "Название_предмета", расписание.ID_предмета);
            return View(расписание);
        }

        // POST: Расписание/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "День_недели,Номер_урока,ID_предмета,ID_класса")] Расписание расписание)
        {
            if (ModelState.IsValid)
            {
                db.Entry(расписание).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Номер_урока = new SelectList(db.Время_уроков, "Номер_урока", "Номер_урока", расписание.Номер_урока);
            ViewBag.ID_класса = new SelectList(db.Классы, "ID_класса", "Номер_класса", расписание.ID_класса);
            ViewBag.ID_предмета = new SelectList(db.Предметы, "ID_предмета", "Название_предмета", расписание.ID_предмета);
            return View(расписание);
        }

        // GET: Расписание/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Расписание расписание = db.Расписание.Find(id);
            if (расписание == null)
            {
                return HttpNotFound();
            }
            return View(расписание);
        }

        // POST: Расписание/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Расписание расписание = db.Расписание.Find(id);
            db.Расписание.Remove(расписание);
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
