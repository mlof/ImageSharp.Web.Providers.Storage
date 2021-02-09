using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Resolvers;
using Storage.Net.Blobs;

namespace PIM.WebService
{
    public class StorageNetImageResolver : IImageResolver
    {
        private readonly IBlobStorage _blobStorage;
        private readonly Blob _blob;
        private readonly ImageMetadata _metadata;

        public StorageNetImageResolver(IBlobStorage blobStorage, Blob blob, ImageMetadata metadata)
        {
            _blobStorage = blobStorage;
            _blob = blob;
            _metadata = metadata;
        }

        /// <inheritdoc />
        public Task<ImageMetadata> GetMetaDataAsync() => Task.FromResult(this._metadata);

        /// <inheritdoc />
        public async Task<Stream> OpenReadAsync()
        {
            return await _blobStorage.OpenReadAsync(_blob.FullPath);
        }
    }
}