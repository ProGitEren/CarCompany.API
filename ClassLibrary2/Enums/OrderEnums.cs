﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public class OrderEnums
    {
        public enum OrderStatus {

            [EnumMember(Value = "Active")]
            Active,
            [EnumMember(Value = "Sold")]
            Sold,
        }

        public enum PaymentMethod {

            [EnumMember(Value = "Advance")]
            Advance,
            [EnumMember(Value = "Installments")]
            Installments,
        }

        public enum OrderType 
        {
            [EnumMember(Value = "Rent")]
            Rent,
            [EnumMember(Value = "Buy")]
            Buybbb,
        }
    }
}