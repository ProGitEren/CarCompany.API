using ClassLibrary2.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Vehicles 
    {
        
        public string Vin { get; set; }

        public string ModelName { get; set; }

        public string EngineName { get; set; }

        public string UserName { get; set; }

        public decimal Averagefuelin { get; set; }

        public decimal Averagefuelout { get; set; }

        [Range(0,250,ErrorMessage = "C02 emission should not exceed value of 250.")]
        public int COemmission { get; set; }

        [Range(0, 200)]
        public int FuelCapacity { get; set; }

        [Range(0, 30000)]
        public int MaxAllowedWeight { get; set; }

        [Range(0, 100)]
        public int MinWeight { get; set; }

        [Range(0,1000)]
        public int BaggageVolume { get; set; }

        public int DrivenKM { get; set; }

        [AllowNull]
        [NotMapped]

        public string ModelCode { get; set; }

        // Navigatipnal Property

        // Foreign Keys
        public int ModelId { get; set; }
        public int EngineId { get; set; }
        public string? UserId { get; set; }


        // Navigation Properties
        public virtual VehicleModels VehicleModel { get; set; }
        public virtual Engines Engine { get; set; }
        public virtual AppUsers User { get; set; }





    }
}
