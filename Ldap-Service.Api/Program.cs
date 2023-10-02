using System.Security.AccessControl;
using Serilog;
using Serilog.Enrichers.AspnetcoreHttpcontext;
using System.IO;
using Ldap_Service.Domain;
using Ldap_Service.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

    builder.Services.AddControllers();
   
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Host.UseSerilog((hostingContext, loggerConfig) =>loggerConfig.ReadFrom.Configuration(hostingContext.Configuration));

    builder.Configuration.AddJsonFile(Path.GetFullPath(@"../appsettings.json"), optional: false, reloadOnChange: true);

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetSection("Redis")["RedisServer"];
    });
    builder.Services.AddTransient<ICacheProvider,CacheProvider>();
    builder.Services.AddTransient<ICurrentUserInfrastructure,CurrentUserInfrastructure>();
    builder.Services.AddTransient<IJwtInfrastructure,JwtInfrastructure>();
    builder.Services.AddTransient<IAuthInfrastructure,AuthInfrastructure>();
    builder.Services.AddTransient<ADConnection,ADConnection>();

    builder.Services.AddSession(o =>
    {
        o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        o.Cookie.Name = "BNT.Session";
        o.Cookie.HttpOnly = true;
    });

    
    var app = builder.Build();
    app.UseSession();
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest);
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.MapControllers();
  
    app.Run();

