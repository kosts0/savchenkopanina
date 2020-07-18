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
        public ActionResult Index(Guid clas,Guid predmet)
        {
            Оцнекикласса оценки = new Оцнекикласса();
            var ученики = db.Ученики.Where(p => p.ID_класса == clas);
            var оценкиset = db.Оценки.Where(p => p.id_предмета == predmet);
            оценки.ID_класса = clas;
            оценки.номер_класса = db.Классы.Where(p => p.ID_класса == clas).FirstOrDefault().Номер_класса;
            foreach(var item in ученики)
            {
                оценки.Ученики.Add(item);
            }

            foreach(var item in оценкиset)
            {
                оценки.Оценки.Add(item);
            }
            return View(оценки);
        }
    }
}