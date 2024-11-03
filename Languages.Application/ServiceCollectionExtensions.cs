using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Infrastructure.DataAccess;
using Core.Interfaces.Repository;
using Core.Interfaces;
using Infrastructure.Repository;
using Infrastructure.UnitofWork;
using Infrastructure.Extensions;
using Application.FrameWork;


namespace Application
{
    public static class ServiceCollectionExtensions
    {

     
        public static IServiceCollection AddLeitnerBox(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddDatabaseContext<LeitnerBoxDbcontext>(config, "LeitnerBox");

            //services.AddTransient<IApiDictionaryRepository, ApiDictionaryRepository>();
            //services.AddTransient<IBoxDataRepository, BoxDataRepository>();
            //services.AddTransient<IUserBoxRepository, UserBoxRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ILeitnerBoxDbcontext, LeitnerBoxDbcontext>();
            return services;
        }
    }
}
