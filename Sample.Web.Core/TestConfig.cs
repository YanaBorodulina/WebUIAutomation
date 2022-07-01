using System;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace Sample.Web.Core
{
    public static class TestConfig
    {
        public static IConfiguration ConfigurationBuilder
        {
            get
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                    .Build();
            }
        }

        public static string UserName
        {
            get { return ConfigurationBuilder[nameof(UserName)]; }
        }

        public static string UserPassword
        {
            get { return ConfigurationBuilder[nameof(UserPassword)]; }
        }

        public static string Browser
        {
            get { return ConfigurationBuilder[nameof(Browser)]; }
        }

        public static int DefaultTimeout
        {
            get { return int.Parse(ConfigurationBuilder[nameof(DefaultTimeout)]); }
        }

        public static int ElementTimeout
        {
            get { return int.Parse(ConfigurationBuilder[nameof(ElementTimeout)]); }
        }

        public static Uri BaseUrl
        {
            get { return new Uri(ConfigurationBuilder[nameof(BaseUrl)]); }
        }

        public static Uri ApiUrl
        {
            get { return new Uri(ConfigurationBuilder[nameof(ApiUrl)]); }
        }
    }
}