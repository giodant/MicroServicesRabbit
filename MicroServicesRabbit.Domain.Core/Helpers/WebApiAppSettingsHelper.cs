using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicroServicesRabbit.Domain.Core.Helpers
{
    public class WebApiAppSettingsHelper
    {
        private static IConfigurationRoot _configuration;
        public static WebApiAppSettingsHelper Instance { get; private set; }

        public static void Initialize(IHostingEnvironment env)
        {
            Initialize(env.EnvironmentName);
        }

        public static void Initialize(string environmentName)
        {
            if (Instance == null)
            {
                Instance = new WebApiAppSettingsHelper();
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                    .AddEnvironmentVariables();
                _configuration = builder.Build();
            }
        }


        public IConfigurationSection GetSection(string name)
        {
            return _configuration.GetSection(name);
        }

        public string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name);
        }
    }
}
