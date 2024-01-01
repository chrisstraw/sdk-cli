using Azure.Storage.Blobs;

namespace Azure.SDK.Blob
{
    public class BlobUtilities
    {
        public async Task DownloadAllContainers(string connectionString, string localPath)
        {
            var client = new BlobServiceClient(connectionString);
            var containers = client.GetBlobContainers();

            foreach (var container in containers)
            {
                await DownloadContainer(connectionString, localPath, container.Name);
            }
        }

        public async Task DownloadContainer(string connectionString, string localPath, string containerName)
        {

            var client = new BlobServiceClient(connectionString);
            var rootPath = Path.Combine(localPath, client.AccountName, containerName);
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            Console.WriteLine($"Downloading container {containerName} to ${rootPath}");

            BlobContainerClient container = client.GetBlobContainerClient(containerName);
            var blobs = container.GetBlobs();
            foreach (var item in blobs)
            {

                var fileName = item.Name.Replace("/", @"\");
                var fileInfo = new FileInfo(Path.Combine(rootPath, fileName));


                if (!Directory.Exists(fileInfo.DirectoryName))
                {
                    Directory.CreateDirectory(fileInfo.DirectoryName);
                }

                
                if (item.Properties.ContentType != null && string.IsNullOrEmpty(fileInfo.Extension))
                {
                    try
                    {
                        var fileExt = MimeMapping.MimeUtility.GetExtensions(item.Properties.ContentType)?.FirstOrDefault();
                        if (fileExt != null)
                        {
                            fileName = string.IsNullOrEmpty(fileInfo.Extension) ? fileInfo.Name : fileInfo.NameWithoutExt();
                            fileName = $"{fileName}.{fileExt ?? "unk"}";
                        }
                    }
                    catch
                    {

                    }

                }



                var blob = container.GetBlobClient(item.Name);
                var result = await blob.DownloadContentAsync();
                result.Value.Content.ToArray().ToFile(Path.Combine(rootPath, fileName));
            }


        }
    }
}
