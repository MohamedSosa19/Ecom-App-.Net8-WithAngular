using AutoMapper;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConnectionMultiplexer _redis;
        private readonly IManagementServices _managementServices;

        public ICategoryRepository CategoryRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public IProductRepository ProductRepository { get; }
        public ICustomerBasketRepository customerBasketRepository { get; }

        public UnitOfWork(AppDbContext context, IManagementServices managementServices, IMapper mapper,IConnectionMultiplexer redis)
        {
            _context = context;
            _managementServices = managementServices;
            _mapper = mapper;
            _redis = redis;
            CategoryRepository = new CategoryRepository(_context);
            PhotoRepository = new PhotoRepository(_context);
            ProductRepository = new ProductRepository(_context,_mapper, _managementServices);
            customerBasketRepository = new CustomerBasketRepository(_redis);
            
        }
    }
}
