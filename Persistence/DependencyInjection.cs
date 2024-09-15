using Application.Interfaces.IRepositories;
using Application.Interfaces.IUnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.AutoMapper;
using Persistence.HealthCheck;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            //Repositories
            #region Register Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            //AutoMapper
            #region Register Automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(MapperConfigurations).Assembly);
            #endregion

            //AppDbContext
            #region Register Dbcontext
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("Db")));

            //services.AddDbContext<AppDbContext>(options =>
            //   options.UseInMemoryDatabase("test"));
            #endregion

            //Other services
            #region Register Other Services
            services.AddHealthChecks()
                    .AddCheck<SqlHeathCheck>("NoteApplication");
            #endregion

            return services;
        }
    }
}
