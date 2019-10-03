using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.Services.Interface
{
   public interface IUploadService
   {
      string UploadImage(IFormFile file, string folderName);
      Task<string> UploadVideo(IFormFile file);
      TimeSpan VideoDuration(string fileName);
      string CreateThumbnail(string videoName);
      void RemoveImage(string imageName, string folderName);
   }
}
