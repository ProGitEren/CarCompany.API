using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.DTO_OrderVehicles
{
    public class AddFileDto
    {
        public int Id { get; set; }
        public IFormFile Picture { get; set; }
        
    }
}
