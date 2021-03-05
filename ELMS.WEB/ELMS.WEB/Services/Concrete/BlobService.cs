using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ELMS.WEB.Helpers;
using ELMS.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ELMS.WEB.Services.Concrete
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient __BlobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            __BlobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            BlobContainerClient _ContainerClient = __BlobServiceClient.GetBlobContainerClient("image");
            BlobClient _BlobClient = _ContainerClient.GetBlobClient(blobName);
            await _BlobClient.DeleteIfExistsAsync();
        }

        public async Task<BlobDownloadInfo> GetBlobAsync(string name)
        {
            BlobContainerClient _ContainerClient = __BlobServiceClient.GetBlobContainerClient("image");
            BlobClient _BlobClient = _ContainerClient.GetBlobClient(name);

            return await _BlobClient.DownloadAsync();
        }

        public async Task<IEnumerable<string>> ListBlobsAsync()
        {
            BlobContainerClient _ContainerClient = __BlobServiceClient.GetBlobContainerClient("image");
            IList<string> _Items = new List<string>();

            await foreach (var blobItem in _ContainerClient.GetBlobsAsync())
            {
                _Items.Add(blobItem.Name);
            }

            return _Items;
        }

        public async Task UploadContentBlobAsync(string content, string fileName)
        {
            BlobContainerClient _ContainerClient = __BlobServiceClient.GetBlobContainerClient("image");
            BlobClient _BlobClient = _ContainerClient.GetBlobClient(fileName);

            byte[] _Bytes = Encoding.UTF8.GetBytes(content);

            using MemoryStream _MemoryStream = new MemoryStream(_Bytes);
            await _BlobClient.UploadAsync(_MemoryStream, new BlobHttpHeaders 
            {
                ContentType = fileName.GetContentType()
            });
        }

        public async Task<bool> UploadFileBlobAsync(string filePath, string fileName)
        {
            BlobContainerClient _ContainerClient = __BlobServiceClient.GetBlobContainerClient("image");
            BlobClient _BlobClient = _ContainerClient.GetBlobClient(fileName);

            try
            {
                await _BlobClient.UploadAsync(filePath, new BlobHttpHeaders
                {
                    ContentType = filePath.GetContentType()
                });
            }
            catch (RequestFailedException exception)
            {
                return false;
            }

            return true;
        }

        public async Task<string> UploadFormFile(IFormFile file)
        {
            StreamReader _StreamReader = new StreamReader(file.OpenReadStream());

            BlobContainerClient _ContainerClient = __BlobServiceClient.GetBlobContainerClient("image");
            BlobClient _BlobClient = _ContainerClient.GetBlobClient($"{Guid.NewGuid()}_{DateTime.Now.ToString(@"yyyy-MM-dd")}_{file.FileName}");

            using (Stream stream = file.OpenReadStream())
            {
                BlobContentInfo _Info = await _BlobClient.UploadAsync(stream);
                return _BlobClient?.Uri?.AbsoluteUri ?? string.Empty;
            }
        }
    }
}
