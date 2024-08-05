using ClassLibrary2.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Vehicles 
    {
        
        public string Vin { get; set; }

        public decimal Averagefuelin { get; set; }

        public decimal Averagefuelout { get; set; }

        [Range(0,250)]
        public int COemmission { get; set; }

        [Range(0, 200)]
        public int FuelCapacity { get; set; }

        [Range(0, 30000)]
        public int MaxAllowedWeight { get; set; }

        [Range(0, 100)]
        public int MinWeight { get; set; }

        [Range(0,1000)]
        public int BaggageVolume { get; set; }


        public int Drivenkm { get; set; }



        // Navigatipnal Property

        public int ModelId { get; set; } // 5 number will be used in vin

        public virtual VehicleModel VehicleModel { get; set; }

        public int EngineId {  get; set; }
        
        public virtual Engine Engine { get; set; }

        public string UserId { get; set; }

        public virtual AppUsers User { get; set; }





    }
}
