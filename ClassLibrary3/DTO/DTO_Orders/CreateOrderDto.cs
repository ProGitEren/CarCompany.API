﻿using Infrastucture.DTO.DTO_OrderVehicles;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Enums.OrderEnums;

namespace Infrastucture.DTO.DTO_Orders
{
    public class CreateOrderDto
    {

        public string? BuyerEmail { get; set; }
        public string SellerEmail { get; set; }
        public OrderType OrderType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public CreateOrderVehicleDto OrderVehicleDto { get; set; }
        public TransferAddress TransferAddress { get; set; }
    }
}