

namespace TestApi.Dependencies;

    public static class DependencyInjection
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Database")));

            return services;
        }


        public static IServiceCollection AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // services.Configure<AppConfig>(configuration.GetSection(nameof(AppConfig)));

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IOtpService, OtpService>();

            services.AddHttpContextAccessor();

            return services;
        }
    }