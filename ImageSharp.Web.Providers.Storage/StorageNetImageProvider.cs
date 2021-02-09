using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Resolvers;
using Storage.Net.Blobs;

namespace PIM.WebService
{
    public class StorageNetImageProvider : IImageProvider
    {
        private readonly IHostEnvironment _environment;
        private readonly IBlobStorage _blobStorage;
        private readonly FormatUtilities _formatUtilities;

        public StorageNetImageProvider(IHostEnvironment environment, IBlobStorage blobStorage,
            FormatUtilities formatUtilities)
        {
            _environment = environment;
            _blobStorage = blobStorage;

            _formatUtilities = formatUtilities;
        }

        /// <inheritdoc/>
        public ProcessingBehavior ProcessingBehavior { get; } = ProcessingBehavior.All;

        /// <inheritdoc/>
        public Func<HttpContext, bool> Match { get; set; } = IsMatch;

        private static bool IsMatch(HttpContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool IsValidRequest(HttpContext context) =>
            _formatUtilities.GetExtensionFromUri(context.Request.GetDisplayUrl()) != null;

        /// <inheritdoc/>
        public async Task<IImageResolver> GetAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            if (!await _blobStorage.ExistsAsync(path))
            {
                return await Task.FromResult<IImageResolver>(null);
            }

            var blob = await _blobStorage.GetBlobAsync(path);


            var metadata = new ImageMetadata(blob.LastModificationTime.Value.UtcDateTime, blob.Size.Value);
            return await Task.FromResult<IImageResolver>(new StorageNetImageResolver(_blobStorage, blob, metadata));
        }
    }
}