using ClassLibrary2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Order : BaseEntity<int>
    {
        public decimal Price { get; set; }

        public DateTime orderedTime { get; set; } = DateTime.Now;

        public OrderStatus 

        
        // Navigational Properties

        public string VehicleID { get; set; }
        public Vehicles Vehicle { get; set; }

        public string SellerId { get; set; }

        public AppUsers Seller { get; set; }

        public string? BuyerID { get; set; }

        public AppUsers Buyer { get; set; }

        

    }
}
