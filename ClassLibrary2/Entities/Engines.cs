using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Engines : BaseEntity<int>
    {
        public Cylinder Cylinder { get; set; }

        public string EngineName { get; set; }

        [Range(0,20,ErrorMessage = " The volume of the Engine should not exceed 20 L .")]
        public decimal Volume { get; set; }

        [Range(0, 3000, ErrorMessage = " The Horse Power of the Engine should not exceed 3000 Hp .")]
        public int Hp { get; set; }

        [Range(0, 25, ErrorMessage = " The Compression Ratio of the Engine should not exceed 25:1 .")]
        public int CompressionRatio { get; set; }

        [Range(0, 4000, ErrorMessage = " The Torque of the Engine should not exceed 4000 N.m .")]
        public int Torque { get; set; }

        [Range(0, 200, ErrorMessage = " The Diameter (Bore) of the Engine should not exceed 200 mm .")]
        public decimal diameterCm { get; set; }

        [StringLength(5,ErrorMessage = "Engine Code should be exactly 5 digits (letter/number)")]
        public string EngineCode { get; set; }
        
        //Navigational Property

        public ICollection<Vehicles> Vehicles { get; set; }


    }
}
