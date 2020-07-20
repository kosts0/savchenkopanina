using practic1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace practic1.Controllers
{
    public class ОценкиController : Controller
    {
        private ps2Entities db = new ps2Entities();
        // GET: Оценки
        public ActionResult Index(Guid clas, Guid predmet)
        {
            Оцнекикласса оценки = new Оцнекикласса();
            var ученики = db.Ученики.Where(p => p.ID_класса == clas);
            var оценкиset = db.Оценки.Where(p => p.id_предмета == predmet);
            оценки.ID_класса = clas;
            оценки.номер_класса = db.Классы.Where(p => p.ID_класса == clas).FirstOrDefault().Номер_класса;
            foreach (var item in ученики)
            {
                оценки.Ученики.Add(item);
            }

            foreach (var item in оценкиset)
            {
                оценки.Оценки.Add(item);
            }
            return View(оценки);
        }

        public ActionResult Create(Guid класс, Guid ученик, DateTime день, Guid предмет, int back)
        {
            List<int> a = new List<int>();
            a.Add(2);
            a.Add(3);
            a.Add(4);
            a.Add(5);
            ViewBag.оценка = new SelectList(a);
            ViewBag.класс = класс;
            ViewBag.ученик = ученик;
            ViewBag.день = день;
            ViewBag.предмет = предмет;
            ViewBag.x = back;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Оценка,DateTime Дата,System.Guid ID_ученика, System.Guid id_предмета,int back, string комментарий)
        {
            Оценки оценка = new Оценки();
            оценка.Оценка = Оценка;
            оценка.id_предмета = id_предмета;
            оценка.ID_ученика = ID_ученика;
            оценка.Дата = Дата;
            оценка.Комментарий = комментарий;
                db.Оценки.Add(оценка);
                db.SaveChanges();
                System.Guid? cl = db.Ученики.Where(p => p.ID_ученика == оценка.ID_ученика).FirstOrDefault().ID_класса;
                return RedirectToAction("Index2","Расписание",new {clas = cl, x = back, predmet  = оценка.id_предмета});


        }
    }
}