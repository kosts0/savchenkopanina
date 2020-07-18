using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace practic1.Models
{
    public class Предметыукласса
    {
        public Предметыукласса()
        {
            this.Предметы = new List<Предметы>();
        }
        public string номер_класса { get; set; }
        public System.Guid ID_класса { get; set; }
        public virtual ICollection<Предметы> Предметы { get; set; }
    }
}