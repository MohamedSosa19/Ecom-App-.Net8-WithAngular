using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services
{
    public interface IManagementServices
    {
        Task<List<string>> AddImageAsync(IFormFileCollection files, string src);//هنا ممكن كل منتج يكون ليه اكتر من صوره عشان كده عملنا  ليست 
        void DeleteImageAsync(string src);
    }
}
