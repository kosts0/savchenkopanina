using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace practic1.Models
{
    public class Неделя_предметы_оценки
    {

        public Неделя_предметы_оценки()
        {
            this.Оценки = new List<Оценки>();
            this.Расписание = new List<Расписание>();
        }
        public virtual DateTime day { get; set; }
        public virtual string day_of_week { get; set; }
        public virtual ICollection<Оценки> Оценки { get; set; }
        public virtual ICollection<Расписание> Расписание { get; set; }
      
        public virtual int x { get; set; }
    }
}