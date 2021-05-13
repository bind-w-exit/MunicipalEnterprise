using System;

namespace MunicipalEnterprise.Data.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int HeatMeter { get; set; }
        public double Cost { get; set; }
        public virtual House House { get; set; }
        public virtual User User { get; set; }
    }
}
