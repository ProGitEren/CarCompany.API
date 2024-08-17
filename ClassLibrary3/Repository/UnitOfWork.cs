using AutoMapper;
using Infrastucture.Data;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
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

        private readonly IFileService _fileService;

        public IAddressRepository AddressRepository { get; }

        public IVehicleRepository VehicleRepository { get; }

        public IVehicleModelRepository VehicleModelRepository { get; }

        public IEngineRepository EngineRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper,IFileService fileService)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
            _fileService = fileService;

            VehicleRepository = new VehicleRepository(_context, _mapper);
            AddressRepository = new AddressRepository(_context);
            VehicleModelRepository = new VehicleModelRepository(_context,_fileProvider,_mapper,_fileService);
            EngineRepository = new EngineRepository(_context, _mapper);


        }

        

    }
}
