using AutoMapper;
using Ecom.Core.DTOS;
using Ecom.Core.Entities.Product;

namespace Ecom.Api.Mapping
{
    public class CategoryMapping:Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
        }
    }
}
