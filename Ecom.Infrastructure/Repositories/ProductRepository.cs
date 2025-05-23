﻿using AutoMapper;
using Ecom.Core.DTOS;
using Ecom.Core.Entities.Product;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IManagementServices _managementServices;
        public ProductRepository(AppDbContext context, IMapper mapper, IManagementServices managementServices) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _managementServices = managementServices;
        }

        public async Task<ReturnProductDto> GetAllAsync(ProductParams productParams)
        {
            var query = _context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .AsNoTracking();


            //Filtering by word
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords = productParams.Search.Split(' ');
                query = query.Where(m => searchWords.All(word =>
                m.Name.ToLower().Contains(word.ToLower())||
                m.Description.ToLower().Contains(word.ToLower())
                ));

            }

            //Filter by category Id
            if (productParams.CategoryId.HasValue)
            {
                query = query.Where(m => m.CategoryId == productParams.CategoryId);
            }

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = (productParams.Sort?.Trim()) switch
                {
                    "PriceAsc" => query.OrderBy(m => m.NewPrice),
                    "PriceDesc" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.Name),
                };
            }

            ReturnProductDto returnProductDto = new ReturnProductDto();

            returnProductDto.TotalCount = query.Count();
            query = query.Skip((productParams.pageSize) * (productParams.PageNumber - 1)).Take(productParams.pageSize);
            returnProductDto.products= _mapper.Map<List<ProductDto>>(query);
            return returnProductDto;
        }

        public async Task<bool> AddAsync(AddProductDto addProductDto)
        {
            var product = _mapper.Map<Product>(addProductDto);
           await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            var imagePath = await _managementServices.AddImageAsync(addProductDto.Photo, addProductDto.Name);
            var photo = imagePath.Select(path => new Photo
            {
                ImageName=path,
                ProductId=product.Id
            }).ToList();

            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto)
        {
            var exsitedProduct = await _context.Products.Include(m => m.Category)
                  .Include(m => m.Photos).
                  FirstOrDefaultAsync(m => m.Id == updateProductDto.Id);
            if(exsitedProduct is null)
            {
                return false;
            }
            _mapper.Map(updateProductDto,exsitedProduct);
            var existedPhoto = await _context.Photos.Where(m => m.ProductId == updateProductDto.Id).ToListAsync();
            foreach (var item in existedPhoto)
            {
                _managementServices.DeleteImageAsync(item.ImageName);
            }
            _context.Photos.RemoveRange(existedPhoto);

            var imagePath = await _managementServices.AddImageAsync(updateProductDto.Photo, updateProductDto.Name);
            var photo = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = updateProductDto.Id
            }).ToList();

            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task DeleteAsync(Product product)
        {
            var photo=await _context.Photos.Where(m=>m.ProductId==product.Id)
                .ToListAsync();
            foreach (var item in photo)
            {
                _managementServices.DeleteImageAsync(item.ImageName);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

        }

    }
}
