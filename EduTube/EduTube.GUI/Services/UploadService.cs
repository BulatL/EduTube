using EduTube.GUI.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace EduTube.GUI.Services
{
   public class UploadService : IUploadService
   {
      private IHostingEnvironment _hostingEnvironment;
      public UploadService(IHostingEnvironment hostingEnvironment)
      {
         _hostingEnvironment = hostingEnvironment;
      }

      public async Task<string> UploadImage(IFormFile file)
      {
         Debug.WriteLine("poceo sa snimanjem fajla" + DateTime.Now);
         string extension = Path.GetExtension(file.FileName);
         var fileName = string.Format(@"{0}{1}", Guid.NewGuid(), extension);
         var uploads = Path.Combine(_hostingEnvironment.WebRootPath, @"profileImages");
         var filePath = Path.Combine(uploads, fileName);
         FileStream fileStream = new FileStream(filePath, FileMode.Create);
         await file.CopyToAsync(fileStream);
         fileStream.Close();
         return fileName;
      }

      public async Task<string> UploadVideo(IFormFile file)
      {
         Debug.WriteLine("poceo sa snimanjem fajla" + DateTime.Now);
         string extension = Path.GetExtension(file.FileName);
         var fileName = string.Format(@"{0}{1}", Guid.NewGuid(), extension);
         var uploads = Path.Combine(_hostingEnvironment.WebRootPath, @"videos");
         var filePath = Path.Combine(uploads, fileName);
         FileStream fileStream = new FileStream(filePath, FileMode.Create);
         await file.CopyToAsync(fileStream);
         fileStream.Close();
         return fileName;
      }
      public TimeSpan VideoDuration(string fileName)
      {
         ShellFile so = ShellFile.FromFilePath(_hostingEnvironment.WebRootPath + "/videos/" + fileName);
         double nanoseconds;
         double.TryParse(so.Properties.System.Media.Duration.Value.ToString(), out nanoseconds);

         TimeSpan time = new TimeSpan();
         if (nanoseconds > 0)
         {
            double seconds = (nanoseconds * 0.0001) / 1000;
            int ttl_seconds = Convert.ToInt32(seconds);
            time = TimeSpan.FromSeconds(ttl_seconds);
         }
         return time;
      }
   }
}
