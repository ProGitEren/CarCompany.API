using ClassLibrary2.Entities;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_Vehicles
{
    public class RegisterVehicleDto
    {



        public decimal Averagefuelin { get; set; }
        public decimal Averagefuelout { get; set; }
        public int COemmission { get; set; }
        public int FuelCapacity { get; set; }
        public int MaxAllowedWeight { get; set; }
        public int MinWeight { get; set; }
        public int BaggageVolume { get; set; }
        public int DrivenKM { get; set; }



    }
}
