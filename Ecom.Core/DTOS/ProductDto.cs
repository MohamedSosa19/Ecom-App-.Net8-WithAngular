using Ecom.Core.Entities.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTOS
{
    public record ProductDto
         (int id,string Name, string Description, decimal NewPrice,string OldPrice, string CategoryName,List<PhotoDto> Photos);


    public record ReturnProductDto
    {
        public List<ProductDto> products { get; set; }
        public int TotalCount { get; set; }
    }
    public record PhotoDto
        (string ImageName,int ProductId);

    public record AddProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public int CategoryId { get; set; }
        public IFormFileCollection Photo{ get; set; }
    }
    public record UpdateProductDto : AddProductDto
    {
        public int Id { get; set; }
    }
}

   
