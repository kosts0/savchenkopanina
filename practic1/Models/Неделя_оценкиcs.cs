using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace practic1.Models
{
    public class Неделя_оценкиcs
    {
        public Неделя_оценкиcs()
        {
            this.Оценки = new List<Оценки>();
            this.расписание_недели = new List<Chesk>();
            this.Days = new List<DateTime>();
        }

        public string номер_класса;
        public System.Guid ID_класса { get; set; }
        public virtual ICollection<Оценки> Оценки { get; set; }
        public virtual ICollection<Chesk> расписание_недели { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public virtual ICollection<DateTime> Days { get; set; }
        
    }
}