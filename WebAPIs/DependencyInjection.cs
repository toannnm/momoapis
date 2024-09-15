using Application.Extensions;
using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IServices;
using Application.Models.Settings;
using Application.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace WebAPIs
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Core Services
            #region Register core services
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            #endregion

            // Register Helper Services
            #region Register Helper Services
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUploadImageService, UploadImageService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMomoService, MomoService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IExportExcelService, ExportExcelService>();
            #endregion

            // Register Hangfire Services
            #region Background Servicces
            services.AddHangfire(config => config.UseMemoryStorage());
            services.AddHangfireServer();
            #endregion

            // Bind data from appsettings.json to Section
            #region Binding Data
            var jwtBinding = new JwtSection();
            configuration.GetSection(nameof(JwtSection)).Bind(jwtBinding);

            var serviceSectionbinding = new ServiceSection();
            configuration.GetSection(nameof(ServiceSection)).Bind(serviceSectionbinding);
            #endregion

            // Register Settings Section
            #region Settings
            var jwtSection = services.Configure<JwtSection>(configuration.GetRequiredSection(nameof(JwtSection)));

            var cloudinarySection = services.Configure<CloudinarySection>(configuration.GetRequiredSection(nameof(CloudinarySection)));

            var mailSection = services.Configure<MailSection>(configuration.GetRequiredSection(nameof(MailSection)));

            var momoSection = services.Configure<MomoSection>(configuration.GetRequiredSection(nameof(MomoSection)));
            #endregion

            // Register HttpClient
            #region HttpClient 
            services.AddHttpClient(serviceSectionbinding.Email, client =>
            {
                client.BaseAddress = new Uri("http://localhost:5119");
            });

            services.AddHttpClient(serviceSectionbinding.Momo, client =>
            {
                client.BaseAddress = new Uri("http://localhost:5119");
            });
            #endregion

            // API Test and API Versioning
            #region Other Configuration
            services.AddControllers()
                    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddLoggingMiddleware();
            #endregion

            // Authentication and Authorization
            #region Authentication and Authorization
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuerSigningKey = false,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBinding.Key)),
                        };
                    });
            services.AddAuthorization();

            #endregion 

            // Swagger for JWT
            #region Swagger config
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "ToanAPIs", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[]{}
                        }
                });
            });

            return services;
            #endregion

        }
    }
}
