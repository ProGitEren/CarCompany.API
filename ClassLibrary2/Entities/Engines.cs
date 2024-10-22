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

        public decimal Volume { get; set; }

        public int Hp { get; set; }

        public int CompressionRatio { get; set; }

        public int Torque { get; set; }

        public decimal diameterCm { get; set; }

        public string EngineCode { get; set; }
        
        //Navigational Property

        public ICollection<Vehicles> Vehicles { get; set; }


    }
}
