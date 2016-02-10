using Amazon;
using Microsoft.Extensions.Configuration;

namespace HelloWorld
{
    public class AwsConfigurator
    {
        public static void ConfigureAws(IConfigurationRoot configuration)
        {
            if (!string.IsNullOrEmpty(configuration["AWS:Region"]))
            {
                AWSConfigs.AWSRegion = configuration["AWS:Region"];
                var region = RegionEndpoint.GetBySystemName(AWSConfigs.AWSRegion);
                AWSConfigs.RegionEndpoint = region;
            }

            if (!string.IsNullOrEmpty(configuration["AWS:Profile"]))
            {
                AWSConfigs.AWSProfileName = configuration["AWS:Profile"];
            }
        }
    }
}
