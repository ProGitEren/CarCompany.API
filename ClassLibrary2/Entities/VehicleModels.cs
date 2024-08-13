using Models.CustomAttributes;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class VehicleModels : BaseEntity<int>
    {

       
        public VehicleType VehicleType { get; set; }

        public string EngineCode { get; set; }

        public string ModelShortName { get; set; }

        public string ModelLongName { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [YearRange(1980)]
        public int ModelYear { get; set; }



        // To be used for VIN

        [CustomAllowedValues(1, 2, 3, 4, 5, 6, 7 , 8 , 9)]
        public int ManufacturedCountry { get; set; } //1 number
        public string Manufacturer { get; set; } //2 letter


        [ValidCharacters("ABCDEFGHJKLMNPRSTVWXY1234567890")]
        public string ManufacturedYear { get; set; } // 1 letter

        [ValidCharacters("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")]
        public string ManufacturedPlant { get; set; }// 1 letter

        [ValidCharacters("0123456789X")]
        public string CheckDigit { get; set; } // 1 letter


        // Navigational Property

        public virtual ICollection<Vehicles> Vehicles { get; set; }

    }
}
