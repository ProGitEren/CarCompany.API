using ClassLibrary2.Entities;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO
{
    public class RegisterVehicleDto
    {
        

        [Required]
        public decimal Averagefuelin { get; set; }

        [Required]
        public decimal Averagefuelout { get; set; }

        [Required]
        public int COemmission { get; set; }

        [Required]
        public int FuelCapacity { get; set; }

        [Required]
        public int MaxAllowedWeight { get; set; }

        [Required]
        public int MinWeight { get; set; }

        [Required]
        public int BaggageVolume { get; set; }

        [Required]
        public int DrivenKM { get; set; }



    }
}
