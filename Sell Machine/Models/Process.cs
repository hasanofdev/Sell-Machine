using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Machine.Models
{
    public class Process
    {
        DateTime TarixSaat;
        public string? Mehsul { get; set; }
        public double Odenen { get; set; }
        public double Qaliq { get; set; }
        public string TarixSaatProp
        {
            get { return TarixSaat.ToLongDateString() + " " + TarixSaat.ToLongTimeString(); }
        }

        public Process(string? mehsul, double odenen, double qaliq)
        {
            Mehsul = mehsul;
            Odenen = odenen;
            Qaliq = qaliq;
            TarixSaat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
    }
}
