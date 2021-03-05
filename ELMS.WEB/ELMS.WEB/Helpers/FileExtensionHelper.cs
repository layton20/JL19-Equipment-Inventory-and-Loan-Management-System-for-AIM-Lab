using Microsoft.AspNetCore.StaticFiles;

namespace ELMS.WEB.Helpers
{
    public static class FileExtensionHelper
    {
        private static readonly FileExtensionContentTypeProvider Provider = new FileExtensionContentTypeProvider();

        public static string GetContentType(this string fileName)
        {
            if (!Provider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
