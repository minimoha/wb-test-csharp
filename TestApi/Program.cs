namespace TestApi;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Host.UseSerilog((context, config) =>
        {
            config.Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(@"Filelogs\AppLogs.log", rollingInterval: RollingInterval.Day)
                .ReadFrom.Configuration(context.Configuration);
        });

        builder.Services.AddDataDependencies(builder.Configuration);
        builder.Services.AddServiceDependencies(builder.Configuration);

        builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }));

        builder.Services.Configure<AppConfig>(builder.Configuration.GetSection(nameof(AppConfig)));


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {

        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("corsapp");


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseMiddleware<RequestResponseLoggingMiddleware>();

        app.MapControllers();

        app.Run();
    }
}

