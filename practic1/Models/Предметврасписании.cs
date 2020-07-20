using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace practic1.Models
{
    public class Предметврасписании
    {
        public int Номер_урока { get; set; }
        public string Название_предмета { get; set; }
        public System.TimeSpan Начало_занятия { get; set; }
        public System.TimeSpan Конец_занятия { get; set; }
    }
}