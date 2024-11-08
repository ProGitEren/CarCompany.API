﻿using Microsoft.AspNetCore.Http;
using Models.CustomAttributes;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_VehicleModels
{
    public class VehicleModelDto
    {

        public int? Id { get; set; }
        public VehicleType VehicleType { get; set; }
        public string EngineCode { get; set; }
        public string ModelShortName { get; set; }
        public string ModelLongName { get; set; }
        public int ModelYear { get; set; }
        public int ManufacturedCountry { get; set; } //1 number
        public string Manufacturer { get; set; } //2 letter
        public string ManufacturedPlant { get; set; }// 1 letter
        public string CheckDigit { get; set; }
        public int Quantity { get; set; }
        public string ModelPicturePath { get; set; }
        public decimal Price { get; set; }




    }
}
