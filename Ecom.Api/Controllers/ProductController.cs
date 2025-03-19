using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.DTOS;
using Ecom.Core.Entities.Product;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var Products = await _unitOfWork.ProductRepository
                .GetAllAsync(x => x.Category, x => x.Photos);
                var result = _mapper.Map<List<ProductDto>>(Products);
                if (Products is null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("GetProductBbyId/{id}")]
        public async Task<IActionResult> GetProductBbyId(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository
                    .GetAsyncById(id,
                     x=>x.Category,x=>x.Photos);

                var result = _mapper.Map<ProductDto>(product);
                if (product is null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddNewProduct")]
        public async Task<IActionResult> AddNewProduct(AddProductDto addProductDto)
        {
            try
            {
                await _unitOfWork.ProductRepository.AddAsync(addProductDto);
                return Ok(new ResponseApi(200));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            try
            {
                await _unitOfWork.ProductRepository.UpdateAsync(updateProductDto);
                return Ok(new ResponseApi(200));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult>DeleteProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetAsyncById(id, x => x.Category, x => x.Photos);
                await _unitOfWork.ProductRepository.DeleteAsync(product);
                return Ok(new ResponseApi(200));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
            
        }
    }
}
