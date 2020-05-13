using BLL;
using BLL.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI
{
    public static class ConfigurationAPI
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddControllers();

            
            ConfigurationBLL.Configure(configuration, services);
        }
    }
}
