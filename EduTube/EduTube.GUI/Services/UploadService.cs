using EduTube.GUI.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using LazZiya.ImageResize;

namespace EduTube.GUI.Services
{
   public class UploadService : IUploadService
   {
      private IHostingEnvironment _hostingEnvironment;
      public UploadService(IHostingEnvironment hostingEnvironment)
      {
         _hostingEnvironment = hostingEnvironment;
      }

      public string UploadImage(IFormFile file, string folderName)
      {
         string extension = Path.GetExtension(file.FileName);
         var fileName = string.Format(@"{0}{1}", Guid.NewGuid(), extension);
         var uploads = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
         var filePath = Path.Combine(uploads, fileName);

         using (Image img = Image.FromStream(file.OpenReadStream()))
         {
            //Scale by height : Scales image by provided height value, auto adjusts new width according to aspect ratio.
            Image resizedImage = ImageResize.ScaleByHeight(img, 150);
            resizedImage.Save(filePath);
         }
         return fileName;
      }

      public async Task<string> UploadVideo(IFormFile file)
      {
         Debug.WriteLine("poceo sa snimanjem fajla" + DateTime.Now);
         string extension = Path.GetExtension(file.FileName);
         var fileName = string.Format(@"{0}{1}", Guid.NewGuid(), extension);
         var folderPath = Path.Combine(_hostingEnvironment.WebRootPath, @"videos");
         var fullFilePath = Path.Combine(folderPath, fileName);
         FileStream fileStream = new FileStream(fullFilePath, FileMode.Create);
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
      
      public string CreateThumbnail(string videoName)
      {
         var videoFolder = Path.Combine(_hostingEnvironment.WebRootPath, @"videos");
         var videoPath = Path.Combine(videoFolder, videoName);

         var thumbnailFolder = Path.Combine(_hostingEnvironment.WebRootPath, @"thumbnails");
         var thumbnailName = string.Format(@"{0}.jpg", Guid.NewGuid());
         var thumbnailPath = Path.Combine(thumbnailFolder, thumbnailName);

         var inputFile = new MediaFile { Filename = videoPath };
         var outputFile = new MediaFile { Filename = thumbnailPath };

         using (var engine = new Engine())
         {
            engine.GetMetadata(inputFile);

            // Saves the frame located on the 5th second of the video.
            var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(5) };
            engine.GetThumbnail(inputFile, outputFile, options);
         }
         return thumbnailName;
      }
   }
}
