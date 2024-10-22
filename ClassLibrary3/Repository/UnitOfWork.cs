using AutoMapper;
using ClassLibrary2.Entities;
using FluentValidation;
using Infrastucture.Data;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Models.Entities;
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

        private readonly UserManager<AppUsers> _userManager; 

        

        public IAddressRepository AddressRepository { get; }

        public IVehicleRepository VehicleRepository { get; }

        public IVehicleModelRepository VehicleModelRepository { get; }

        public IEngineRepository EngineRepository { get; }

        public IUserRepository UserRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderVehicleRepository OrderVehicleRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper,IFileService fileService,UserManager<AppUsers> userManager)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
            _fileService = fileService;
            _userManager = userManager;

            VehicleRepository = new VehicleRepository(_context, _mapper);
            AddressRepository = new AddressRepository(_context);
            VehicleModelRepository = new VehicleModelRepository(_context,_fileProvider,_mapper,_fileService);
            EngineRepository = new EngineRepository(_context, _mapper);
            UserRepository = new UserRepository(_context);
            OrderRepository = new OrderRepository(_context,_mapper,_userManager);
            OrderVehicleRepository = new OrderVehicleRepository(_mapper,_context,_fileService);
        }

        

    }
}
