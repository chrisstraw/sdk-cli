using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using AWS.SDK.Models;

namespace AWS.SDK.SystemManager;

public class ParameterStore
{


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceProfile"></param>
    /// <param name="destinationProfile"></param>
    /// <param name="path">"/PathName"</param>
    /// <returns></returns>
    public async Task CopyToAnotherAccountAsync(AwsProfileKey sourceProfile, AwsProfileKey destinationProfile, string path)
    {
        var request = new GetParametersByPathRequest
        {
            Path = path,
            WithDecryption = true,
            Recursive = true,
        };

        string? nextToken = null;

        using (var client = new AmazonSimpleSystemsManagementClient(sourceProfile.AccessKey, sourceProfile.SecretKey, sourceProfile.RegionEndpoint))
        {
            do
            {
                try
                {
                    var response = await client.GetParametersByPathAsync(request);
                    request.NextToken = response.NextToken;
                    nextToken = response.NextToken;

                    Console.WriteLine();
                    Console.WriteLine("********");
                    Console.WriteLine();

                    foreach (var param in response.Parameters)
                    {
                        Console.WriteLine($"Writing Param: {param.Name} = {param.Value}");
                        await AddParameterAsync(destinationProfile, param.Name, param.Value, param.Type);
                    }
                    Console.WriteLine();

                }
                catch (AmazonSimpleSystemsManagementException e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }

            } while (nextToken != null);
        }
    }

    public async Task AddParameterAsync(AwsProfileKey destinationProfile, string parameterName, string parameterValue, ParameterType type)
    {
        using (var client = new AmazonSimpleSystemsManagementClient(destinationProfile.AccessKey, destinationProfile.SecretKey, destinationProfile.RegionEndpoint))
        {
            var request = new PutParameterRequest
            {
                Name = parameterName,
                Value = parameterValue,
                Type = type,
                Overwrite = true // Set to false if you don't want to overwrite an existing parameter
            };

            try
            {
                var response = await client.PutParameterAsync(request);
                Console.WriteLine("Parameter added. Version: " + response.Version);
            }
            catch (AmazonSimpleSystemsManagementException e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
