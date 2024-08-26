using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class OrderVehicle : BaseEntity<int>
    {
        public string VehicleId { get; set; }

        public string ModelName { get; set; }

        public string PictureFolderPath { get; set; }

        public decimal Price { get; set; }

        // Navigational Proeprties

        public virtual Order Order { get; set; }

    }
}
