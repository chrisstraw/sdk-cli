using Amazon;

namespace AWS.SDK.Models
{
    public class AwsProfileKey
    {
        public string? Profile { get; set; }
        public string? Region { get; set; }
        public string? AccessKey { get; set; }
        public string? SecretKey { get; set; }
        public RegionEndpoint RegionEndpoint => RegionEndpoint.GetBySystemName(Region);
    }
}
