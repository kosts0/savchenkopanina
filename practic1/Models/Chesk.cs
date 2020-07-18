using practic1.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace practic1.Models
{

    public class Chesk
    {
        public Chesk()
        {
            this.Предметы = new List<Предметврасписании>();
        }
        public string День_недели { get; set; }
        public System.Guid ID_класса { get; set; }
        public string Номер_класса { get; set; }
        public virtual ICollection<Предметврасписании> Предметы { get; set; }
        public int x;
    }
}