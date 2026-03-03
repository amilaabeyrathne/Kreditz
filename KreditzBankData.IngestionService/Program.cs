using KreditzBankData.IngestionService.Options;
using KreditzBankData.IngestionService.Services;
using KreditzBankData.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

// ── Bootstrap Serilog early so startup errors are captured ──────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting KreditzBankData IngestionService");

    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

    // ── Configuration ────────────────────────────────────────────────────────
    builder.Services.Configure<IngestionOptions>(builder.Configuration.GetSection("IngestionOptions"));

    // ── Serilog (replaces default Microsoft logging) ─────────────────────────
    builder.Services.AddSerilog((services, loggerConfig) => loggerConfig
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // ── EF Core / Postgres ───────────────────────────────────────────────────
    builder.Services.AddDbContext<KreditzBankDataDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("KreditzBankData"),
            npgsql => npgsql.MigrationsAssembly("KreditzBankData.Shared")));

    // ── Application Services ─────────────────────────────────────────────────
    builder.Services.AddMemoryCache();

    // ── Background worker ────────────────────────────────────────────────────
    builder.Services.AddHostedService<FileWatcherBackgroundService>();

    IHost host = builder.Build();

    // Apply any pending migrations on startup
    using (IServiceScope scope = host.Services.CreateScope())
    {
        KreditzBankDataDbContext db = scope.ServiceProvider
            .GetRequiredService<KreditzBankDataDbContext>();
        await db.Database.MigrateAsync();
    }

    await host.RunAsync();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "IngestionService terminated unexpectedly");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
