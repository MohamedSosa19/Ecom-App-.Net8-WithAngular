using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Ecom.Infrastructure.Repositories.Services
{
    public class ManagementServices : IManagementServices
    {
        private readonly IFileProvider _fileProvider;

        public ManagementServices(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            List<string> SaveImageSrc = new List<string>();
            var ImageDirctory = Path.Combine("wwwroot", "Images", src);
            if (Directory.Exists(ImageDirctory) is not true)
            {
                Directory.CreateDirectory(ImageDirctory);
            }
                foreach (var item in files)
                {
                    if (item.Length > 0)
                    {
                        // get Image Name
                        var ImageName = item.FileName;
                        var ImageSrc = $"/Images/{src}/{ImageName}";
                        var root = Path.Combine(ImageDirctory, ImageName);
                        using (FileStream stream = new FileStream(path: root, mode: FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                        }
                        SaveImageSrc.Add(ImageSrc);
                    }

                }
                return SaveImageSrc;
        }
        public void DeleteImageAsync(string src)
        {
            var Info = _fileProvider.GetFileInfo(src);
            var root = Info.PhysicalPath;
            File.Delete(root);
        }
    }
}
