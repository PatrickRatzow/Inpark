using System.Reflection;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Zeta.Common.Api;
using Zeta.Common.Api.Translator;
using Zeta.Common.Api.Versioning;
using Zeta.Inpark.Common;
using Zeta.Inpark.Features.Animals.AalborgZoo;
using Zeta.Inpark.Features.Animals.Interfaces;
using Zeta.Inpark.Features.OpeningHours.AalborgZoo;
using Zeta.Inpark.Features.OpeningHours.Interfaces;
using Zeta.Inpark.Features.ParkEvents.AalborgZoo;
using Zeta.Inpark.Features.ParkEvents.Interfaces;
using Zeta.Inpark.Features.Speaks.AalborgZoo;
using Zeta.Inpark.Features.Speaks.Interfaces;
using Zeta.Inpark.Services;
using Zeta.UI.Transformers;

namespace Zeta.Inpark;

public static class DependencyInjection
{
    public static void AddInpark(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("InparkConnection");
        services.AddDbContext<InparkDbContext>(options =>
        {
            options.UseSqlServer(dbConnection);
        });

        services.AddMemoryCache();
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddVersioning();
        services.AddPipelines();
        services.AddClock();
        services.AddHangFire(dbConnection);
        services.AddTranslator(configuration);
        services.AddZetaUITransformation();
        services.AddSingleton<IEventPublisher, EventPublisher>();

        services.AddSingleton<IHtmlTransformer, HtmlTransformer>();
        
        services.AddSingleton<IAnimalRepository, AalborgZooAnimalRepository>();
        services.AddHttpClient<IAnimalRepository, AalborgZooAnimalRepository>(AalborgZooHttpClient)
            .AddPolicyHandler(GetRetryPolicy());
        services.AddSingleton<IAnimalMapper, AalborgZooAnimalMapper>();

        services.AddScoped<IOpeningHoursRepository, AalborgZooOpeningHoursRepository>();
        services.AddHttpClient<IOpeningHoursRepository, AalborgZooOpeningHoursRepository>(AalborgZooHttpClient)
            .AddPolicyHandler(GetRetryPolicy());
        services.AddSingleton<IOpeningHoursMapper, AalborgZooOpeningHoursMapper>();

        services.AddScoped<ISpeaksRepository, AalborgZooSpeaksRepository>();
        services.AddHttpClient<ISpeaksRepository, AalborgZooSpeaksRepository>(AalborgZooHttpClient)
            .AddPolicyHandler(GetRetryPolicy());
        services.AddSingleton<ISpeaksMapper, AalborgZooSpeaksMapper>();

        services.AddScoped<IParkEventRepository, AalborgZooParkEventRepository>();
        services.AddHttpClient<IParkEventRepository, AalborgZooParkEventRepository>(AalborgZooHttpClient)
            .AddPolicyHandler(GetRetryPolicy());
        services.AddSingleton<IParkEventMapper, AalborgZooParkEventMapper>();

        services.AddResponseMapper();
    }

    public static void UseInpark(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors(options =>
            options.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
        );
        
        app.UseVersioning();
        app.UseResponseMapper();
        app.UseTranslator();
        
        RunMigrations(app.ApplicationServices);

        if (env.IsDevelopment())
        {
            app.UseHangfireDashboard();
        }
        else
        {
            // Force JobStorage to be resolved outside development
            // Not having JobStorage setup will cause RecurringJob to fail
            app.ApplicationServices.GetRequiredService<JobStorage>();
        }
        /*
        RecurringJob.AddOrUpdate<AalborgZooParkEventsJob>(
            x => x.Execute(),
            "* 3 * * *", // Every day at 3 AM 
            TimeZoneInfo.Local
        );
        RecurringJob.AddOrUpdate<AalborgZooOpeningHoursJob>(
            x => x.Execute(),
            "* 3 * * *", // Every day at 3 AM 
            TimeZoneInfo.Local
        );
        RecurringJob.AddOrUpdate<AalborgZooUpdateSpeaksJob>(
            x => x.Execute(),
            "* 3 * * *", // Every day at 3 AM 
            TimeZoneInfo.Local
        );
        */
    }

    private static IServiceCollection AddHangFire(this IServiceCollection services, string connectionString)
    {
        // Add Hangfire services.
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
           
            }));

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;
    }
    
    private static void RunMigrations(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();

        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<InparkDbContext>();

            if (context.Database.IsSqlServer())
                context.Database.Migrate();
            else
                throw new ApplicationException("Database is not SQL Server or connection couldn't be established");
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<IHost>>();

            logger.LogError(ex, "An error occurred while migrating or seeding the database");

            throw;
        }
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }

    private static void AalborgZooHttpClient(HttpClient client)
    {
        client.BaseAddress = new("https://api.aalborgzoo.dk/api/");
    }
}