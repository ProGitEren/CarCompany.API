using AutoMapper;
using Infrastucture.Data;
using Infrastucture.Interface.Repository_Interfaces;
using Microsoft.Extensions.FileProviders;
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

        private readonly IFileProvider _fileProvider;

        private readonly IMapper _mapper;

        public IAddressRepository AddressRepository { get; }

        public IVehicleRepository VehicleRepository { get; }

        public IVehicleModelRepository VehicleModelRepository { get; }

        public IEngineRepository EngineRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;

            VehicleRepository = new VehicleRepository(_context);
            AddressRepository = new AddressRepository(_context);
            VehicleModelRepository = new VehicleModelRepository(_context,fileProvider,mapper);
            EngineRepository = new EngineRepository(_context);


        }

        

    }
}
