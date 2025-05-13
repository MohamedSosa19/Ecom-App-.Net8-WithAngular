using Ecom.Core.Entities;
using Ecom.Core.interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class CustomerBasketRepository : ICustomerBasketRepository
    {
        private readonly IDatabase _database;

        public CustomerBasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public Task<bool> DeleteAsync(string id)
        {
            return _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var result = await _database.StringGetAsync(id);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<CustomerBasket>(result);
            }
            return null;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var _basket=await _database.StringSetAsync(customerBasket.Id,JsonSerializer.Serialize(customerBasket),TimeSpan.FromDays(3));
            if (customerBasket !=null)
            {
                await _database.StringGetAsync(customerBasket.Id);
            }
            return null;
        }
    }
}
