using AutoMapper;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BugController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetNotFound")]
        public async Task<IActionResult> GetNotFound()
        {
            var category = await _unitOfWork.CategoryRepository.GetAsyncById(100);
            if (category is null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet("GetServerError")]
        public async Task<IActionResult> GetServerError()
        {
            var category = await _unitOfWork.CategoryRepository.GetAsyncById(100);
            category.Name = "";
            return Ok(category);
        }

        [HttpGet("GetBadRequest/{Id}")]
        public async Task<IActionResult> GetBadRequest(int id)
        {
            return Ok();
        }

        [HttpGet("GetBadRequest")]
        public async Task<IActionResult> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
