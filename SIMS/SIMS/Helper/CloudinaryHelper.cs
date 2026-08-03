using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace SIMS.Helpers
{
    public static class CloudinaryHelper
    {
        public static async Task<string?> UploadFileAsync(IFormFile file, Cloudinary cloudinary)
        {
            if (file == null || file.Length == 0) return null;

            using var stream = file.OpenReadStream();
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "SIMS/Assignment",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = true
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl?.ToString();
        }

        public static async Task<bool> DeleteFileAsync(string fileUrl, Cloudinary cloudinary)
        {
            if (string.IsNullOrEmpty(fileUrl)) return false;

            try
            {
                var uri = new Uri(fileUrl);
                var segments = uri.Segments;
                int uploadIndex = Array.FindIndex(segments, s => s.StartsWith("upload"));

                if (uploadIndex != -1 && segments.Length > uploadIndex + 2)
                {
                    var publicId = string.Join("", segments.Skip(uploadIndex + 2)).Trim('/');
                    publicId = Uri.UnescapeDataString(publicId);

                    var deletionParams = new DeletionParams(publicId)
                    {
                        ResourceType = ResourceType.Raw
                    };

                    var result = await cloudinary.DestroyAsync(deletionParams);
                    return result.Result == "ok";
                }
            }
            catch
            {
            }
            return false;
        }
    }
}