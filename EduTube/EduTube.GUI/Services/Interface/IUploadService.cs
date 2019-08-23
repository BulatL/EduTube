using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.Services.Interface
{
   public interface IUploadService
   {
      Task<string> UploadImage(IFormFile file);
      Task<string> UploadVideo(IFormFile file);
      TimeSpan VideoDuration(string fileName);
   }
}
