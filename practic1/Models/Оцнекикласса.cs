using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace practic1.Models
{
    public class Оцнекикласса
    {
        public Оцнекикласса()
        {
            this.Оценки = new List<Оценки>();
            this.Ученики = new List<Ученики>();

        }
        public string номер_класса;
        public System.Guid ID_класса { get; set; }
        public virtual ICollection<Оценки> Оценки { get; set; }
        public virtual ICollection<Ученики> Ученики { get; set; }
    }
}