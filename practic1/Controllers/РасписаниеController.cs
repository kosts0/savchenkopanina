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
        public Chesk Index(Guid clas,string this_day,int? x)
        {
            Chesk расписаниеModel = new Chesk();
            var расписание = db.Расписание.Include(р => р.Время_уроков).Include(р => р.Классы).Include(р => р.Предметы).Where(p => p.ID_класса == clas).Where(p => p.День_недели == this_day);
            bool t = false;
            List<Предметы> предметы = new List<Предметы>();
            расписаниеModel.ID_класса = clas;
            расписаниеModel.День_недели = this_day;
            расписаниеModel.x = x;
            расписаниеModel.Номер_класса = db.Классы.Where(p=> p.ID_класса == clas).FirstOrDefault().Номер_класса;
            foreach (var item in расписание)
            {
                Предметврасписании предметitem = new Предметврасписании();
                предметitem.Название_предмета = item.Предметы.Название_предмета;
                предметitem.Номер_урока = item.Номер_урока;
                //предметitem.Начало_занятия = item.Время_уроков.Конец_занятия;
                //предметitem.Конец_занятия = item.Время_уроков.Конец_занятия;
                t = true;
                расписаниеModel.Предметы.Add(предметitem);
            }
            if (t == false)
            {
                
                for (int i = 1; i <= 4; i++)
                {
                    Предметврасписании предметitem = new Предметврасписании();
                    предметitem.Номер_урока = i;
                    предметitem.Название_предмета = "Нет занятия";
                    //предметitem.Начало_занятия = db.Время_уроков.Where(p => p.Номер_урока == i).FirstOrDefault().Начало_занятия;
                    //предметitem.Конец_занятия = db.Время_уроков.Where(p => p.Номер_урока == i).FirstOrDefault().Конец_занятия;
                   
                    расписаниеModel.Предметы.Add(предметitem);
                }
            }
            

            return расписаниеModel;
        }


        public ActionResult Index2(Guid clas,int x, Guid student)
        {
            Неделя_оценкиcs week = new Неделя_оценкиcs();
            System.DateTime date = DateTime.Now;
            int k = 0;
            while(date.DayOfWeek.ToString() != "Monday")
            {
                date = DateTime.Now.AddDays(-1*k);
                k++;

            }

            DateTime l = date;
            k = 0;
            while(date.DayOfWeek.ToString() != "Sunday")
            {
                Chesk chesk = Index(clas, date.DayOfWeek.ToString(),0);
                week.Days.Add(date);
                week.расписание_недели.Add(chesk);
                date = DateTime.Now.AddDays(-1 * k);
                k++;
            }
            DateTime r = date;
            week.ID_класса = clas;
            week.номер_класса = db.Классы.Where(p => p.ID_класса == clas).FirstOrDefault().Номер_класса;
            var оценки = db.Оценки.Where(p => p.ID_ученика == student);
            foreach(var item in оценки)
            {
                if(item.Дата >=l && item.Дата <= r)
                {
                    week.Оценки.Add(item);
                }
            }
            return View(week);
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
