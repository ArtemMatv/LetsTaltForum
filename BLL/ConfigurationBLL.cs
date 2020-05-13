using DAL;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BLL.Interfaces;
using BLL.Services;
using BLL.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BLL
{
    public static class ConfigurationBLL
    {
        static public void Configure(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<ForumContext>(service =>
                service.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            IConfigurationSection appSettingsSection;
            if (configuration != null)
            {
                appSettingsSection = configuration.GetSection("AppSettings");
                services.Configure<AppSettings>(appSettingsSection);

                var appSettings = appSettingsSection.Get<AppSettings>();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            }


            ConfigureScoped(services);
        }

        private static void ConfigureScoped(IServiceCollection services)
        {
            services.AddTransient<IUsersService, UserService>();
            services.AddTransient<ITopicService, TopicService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();

            services.AddTransient<IUnitOfWork<User, IdentityRole<int>>, UnitOfWork<User, IdentityRole<int>>>();
            services.AddTransient<IUnitOfWork<User, Post>, UnitOfWork<User, Post>>();
            services.AddTransient<IUnitOfWork<Comment, Post>, UnitOfWork<Comment, Post>>();

            services.AddTransient<IRepository<Topic>, Repository<Topic>>();
            services.AddTransient<IRepository<User>, Repository<User>>();
        }
    }
}
