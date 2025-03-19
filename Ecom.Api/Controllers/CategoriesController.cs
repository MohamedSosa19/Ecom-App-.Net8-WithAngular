using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.DTOS;
using Ecom.Core.Entities.Product;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllCategories")]
        public async Task <IActionResult> GetAllCategories()
        {
            try
            {
                var Categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                if (Categories is null)
                    return BadRequest(new ResponseApi(400));
                return Ok(Categories);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetAsyncById(id);
                if (category is null)
                    return BadRequest(new ResponseApi(400,$"Not Found CategoryId {id}"));
                return Ok(category);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddNewCategory")]
        public async Task<IActionResult> AddNewCategory(CategoryDto categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                await _unitOfWork.CategoryRepository.AddAsync(category);
                return Ok("Item Added Successfully");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                var category = new Category()
                {
                    Name = updateCategoryDto.Name,
                    Description = updateCategoryDto.Description,
                    Id = updateCategoryDto.Id
                };
                await _unitOfWork.CategoryRepository.UpdateAsync(category);
                return Ok("Item Updated Successfully");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _unitOfWork.CategoryRepository.DeleteAsync(id);
                return Ok("Item Deleted Successfully");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
