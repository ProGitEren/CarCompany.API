using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface
{
    public interface IUnitOfWork
    {
        public IAddressRepository AddressRepository { get; }

        public IVehicleRepository VehicleRepository { get; }
       
    }
}
