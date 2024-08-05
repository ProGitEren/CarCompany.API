using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Engine : BaseEntity<int>
    {
        public enum Cylinder
        {
            i2,
            i4,
            i6,
            v4,
            v6,
            v8,
            v10,
            v12

        }

        public string VinCode { get; set; } // to be used for vin
        public decimal Volume { get; set; }

        public int Hp { get; set; }

        public int CompressionRate { get; set; }

      
        public int Torque { get; set; }

      
        public decimal diameterCm { get; set; }

        //Navigational Property

        public ICollection<Vehicles> Vehicles { get; set; }


    }
}
