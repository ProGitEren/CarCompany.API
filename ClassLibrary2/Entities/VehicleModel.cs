using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class VehicleModel : BaseEntity<int>
    {

       
        public VehicleType VehicleType { get; set; }

        public string ModelCode { get; set; }

        public string ModelShortName { get; set; }

        public string ModelLongName { get; set; }


        public int Quantity { get; set; }



        // To be used for VIN

        public int ManufacturedCountry { get; set; } //1 number
        public string Manufacturer { get; set; } //2 letter

        public string securityCode { get; set; } // 1 letter

        public string ManufacturedYear { get; set; } // 1 letter

        public string ManufacturedPlant { get; set; }// 1 letter

        public string CheckDigit { get; set; }


        // Navigational Property

        public virtual ICollection<Vehicles> Vehicles { get; set; }

    }
}
