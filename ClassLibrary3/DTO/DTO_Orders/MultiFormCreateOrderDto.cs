using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Enums.OrderEnums;

namespace Infrastucture.DTO.DTO_Orders
{
    public class MultiFormCreateOrderDto
    {
        public string? BuyerEmail { get; set; }
        public string SellerEmail { get; set; }
        //public DateTime? OrderedDate { get; set; } = DateTime.Now;
        //public OrderStatus OrderStatus { get; set; } = OrderStatus.Active;
        public OrderType OrderType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string OrderVehicleDtoJson { get; set; }
        public string TransferAddressJson { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
