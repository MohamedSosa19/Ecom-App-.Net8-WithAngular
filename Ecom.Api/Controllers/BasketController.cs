using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BasketController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet("GetBasketById/{id}")]
        public async Task<IActionResult> GetBasketById(string id)
        {
            var result = await _unitOfWork.customerBasketRepository.GetBasketAsync(id);
            if(result is null)
            {
                return Ok(new CustomerBasket());
            }
            return Ok(result);
        }

        [HttpPost("UpdateBasket")]
        public async Task<IActionResult> UpdateBasket(CustomerBasket customerBasket)
        {
            var basket = await _unitOfWork.customerBasketRepository.UpdateBasketAsync(customerBasket);
            return Ok(basket);
        }

        [HttpDelete("DeleteBasketById/{id}")]
        public async Task<IActionResult> DeleteBasketById(string id)
        {
            var result = await _unitOfWork.customerBasketRepository.DeleteAsync(id);
            return Ok(result);
        }
    }
}
