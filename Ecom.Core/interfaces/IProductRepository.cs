using Ecom.Core.DTOS;
using Ecom.Core.Entities.Product;
using Ecom.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<ReturnProductDto> GetAllAsync(ProductParams productParams);
        Task<bool> AddAsync(AddProductDto addProductDto);
        Task<bool> UpdateAsync(UpdateProductDto updateProductDto);
        Task DeleteAsync(Product product);
    }

}
