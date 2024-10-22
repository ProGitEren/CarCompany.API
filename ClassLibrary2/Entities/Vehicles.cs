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

        public int COemmission { get; set; }

        public int FuelCapacity { get; set; }

        public int MaxAllowedWeight { get; set; }

        public int MinWeight { get; set; }

        public int BaggageVolume { get; set; }

        public int DrivenKM { get; set; }

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
