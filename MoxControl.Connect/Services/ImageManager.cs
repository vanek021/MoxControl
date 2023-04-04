using MoxControl.Connect.Data;
using MoxControl.Connect.Models.Entities;
using MoxControl.Core.Services.BucketStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Services
{
    public class ImageManager
    {
        private readonly IBucket _bucket;
        private readonly ConnectDatabase _connectDatabase;

        public ImageManager(ConnectDatabase connectDatabase, IBucketStorageService bucketStorageService) 
        {
            _connectDatabase = connectDatabase;
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ISOImage image)
        {
            _connectDatabase.ISOImages.Update(image);

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
    }
}
