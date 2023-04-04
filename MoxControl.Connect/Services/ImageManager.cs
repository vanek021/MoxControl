using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using MoxControl.Connect.Data;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Services.BucketStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Services
{
    public class ImageManager
    {
        private readonly IBucket _bucket;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ConnectDatabase _connectDatabase;

        public ImageManager(ConnectDatabase connectDatabase, IBucketStorageService bucketStorageService, IWebHostEnvironment webHostEnvironment) 
        {
            _connectDatabase = connectDatabase;
            _webHostEnvironment = webHostEnvironment;
            _bucket = bucketStorageService.GetBucket("isoimages");
        }

        public async Task<List<ISOImage>> GetAllAsync()
        {
            return await _connectDatabase.ISOImages.GetAll();
        }

        public async Task<ISOImage> GetByIdAsync(long id)
        {
            return await _connectDatabase.ISOImages.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(ISOImage image)
        {
            _connectDatabase.ISOImages.Insert(image);

            try
            {
                await _connectDatabase.SaveChangesAsync();
                
                if (image.StorageMethod == ImageStorageMethod.Local)
                    BackgroundJob.Enqueue<ImageManager>(x => x.HangfireDownloadImage(image.Id));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ISOImage image)
        {
            var oldPath = await _connectDatabase.ISOImages.GetImagePath(image.Id);

            var isNeedToDownload = IsNeedToDownload(image, oldPath);

            if (isNeedToDownload)
                image.DownloadSuccess = false;

            _connectDatabase.ISOImages.Update(image);

            try
            {
                await _connectDatabase.SaveChangesAsync();

                if (isNeedToDownload)
                    BackgroundJob.Enqueue<ImageManager>(x => x.HangfireDownloadImage(image.Id));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var image = await _connectDatabase.ISOImages.GetByIdAsync(id);

            if (image is null)
                return false;

            _connectDatabase.ISOImages.Delete(image);

            try
            {
                await _connectDatabase.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task HangfireDownloadImage(long imageId)
        {
            var image = await _connectDatabase.ISOImages.GetByIdAsync(imageId);
            var path = Path.Combine($"image{image.Id}", "image.iso");
            if (!string.IsNullOrEmpty(image.ImagePath))
            {
                if (_bucket.ContainsObject(path))
                    _bucket.DeleteObject(path);

                using var client = new HttpClient();

                var response = await client.GetAsync(image.ImagePath);

                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", path);

                using var fs = new FileStream(fullPath, FileMode.CreateNew);

                await response.Content.CopyToAsync(fs);

                image.DownloadSuccess = true;
                image.ImagePath = _bucket.GetPublicURL(path);

                _connectDatabase.ISOImages.Update(image);
                await _connectDatabase.SaveChangesAsync();
            }
        }

        private static bool IsNeedToDownload(ISOImage image, string? oldPath)
        {
            return oldPath != image.ImagePath && image.StorageMethod == ImageStorageMethod.Local;
        }
    }
}
