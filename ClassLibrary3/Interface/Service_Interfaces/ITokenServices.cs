using ClassLibrary2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Service_Interfaces
{
    public interface ITokenServices
    {
        Task<string> CreateToken(AppUsers appusers);
    }
}
