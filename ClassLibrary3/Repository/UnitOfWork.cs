using Infrastucture.Data;
using Infrastucture.Interface.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class UnitOfWork : IUnitOfWork 
    {

        private readonly ApplicationDbContext _context;

        private readonly IUnitOfWork _unitOfWork;

        public IAddressRepository AddressRepository { get; }

        public IVehicleRepository VehicleRepository { get; }

        public IVehicleModelRepository VehicleModelRepository { get; }

        public IEngineRepository EngineRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            VehicleRepository = new VehicleRepository(_context);
            AddressRepository = new AddressRepository(_context);
            VehicleModelRepository = new VehicleModelRepository(_context);
            EngineRepository = new EngineRepository(_context);


        }

        

    }
}
