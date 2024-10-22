using ClassLibrary2.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Enums.OrderEnums;

namespace Models.Entities
{
    public class Order : BaseEntity<int>
    {

        [EmailAddress(ErrorMessage = "Not valid Email Address")]
        public string? BuyerEmail { get; set; }

        [EmailAddress(ErrorMessage = "Not valid Email Address")]
        public string SellerEmail { get; set; }
        public DateTime OrderedDate { get; set; } = DateTime.Now;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Active;
        public OrderType OrderType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        
        public TransferAddress TransferAddress { get; set; }

        public bool IsVehicleOwnerTransferred { get; set; } = false;
        // Navigational Properties

        //public string VehicleID { get; set; }
        //public Vehicles Vehicle { get; set; }

        //public string SellerId { get; set; }  

        //public AppUsers Seller { get; set; }

        //public string? BuyerID { get; set; }

        //public AppUsers Buyer { get; set; }
        //public Guid AddressId { get; set; }
        //public Address TransferAddress { get; set; }
        
        public int OrderVehicleId { get; set; }
        public virtual OrderVehicle Vehicle { get; set; }
    }
}
