using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_Users
{
    public class ParamsUserDto
    {
        public int TotalItems { get; set; }

        public int PageItemCount { get; set; }

        public List<UserwithdetailsDto> UserDtos { get; set; }
    }
}
