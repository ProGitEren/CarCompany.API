using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Repository_Interfaces
{
    public interface IUnitOfWork 
    {
        public IAddressRepository AddressRepository { get; }

        public IVehicleRepository VehicleRepository { get; }

        public IVehicleModelRepository VehicleModelRepository { get; }

        public IEngineRepository EngineRepository { get; }

    }
}
