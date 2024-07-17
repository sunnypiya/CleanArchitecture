using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PTG.NextStep.Tenant;
using PTG.NextStep.API.MockData;
using PTG.NextStep.Database;
using PTG.NextStep.Database.Search;
using PTG.NextStep.Domain;
using PTG.NextStep.Service;
using PTG.NextStep.Service.Extensions;
using Serilog;
using PTG.NextStep.Service.Validators;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using PTG.NextStep.API.SignalR;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var environment = builder.Environment;

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("NextStep"));
configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddDistributedMemoryCache();
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("NextStep:RedisCache:ConnectionString");
//    options.InstanceName = "Development";
//});

//builder.Services.AddSingleton<IAppSettings>(serviceProvider =>
//serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value);
// Add services to the container.

builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddSingleton<IMultiTenancyService, MultiTenancyService>();
builder.Services.AddScoped<ISearchService, SearchService>();

builder.Services.AddScoped<IDeductionService, DeductionService>();
builder.Services.AddScoped<IDeductionRepository, DeductionRepository>();

builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<ILookupRepository, LookupRepository>();
builder.Services.AddSingleton<IHubService, HubService>();

// Register the task queue and background service

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddSingleton<ITaskQueue, TaskQueue>();
builder.Services.AddHostedService<NSBackgroundService>();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

// Configure Serilog from appsettings.json
builder.Host.UseSerilog((context, configuration) => {
    configuration.ReadFrom.Configuration(context.Configuration);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<TenantDbContext>(options =>
//{
//    var azureServiceTokenProvider = new DefaultAzureCredential();
//    var keyVaultEndpoint = new Uri(configuration["KeyVault:VaultUri"]);
//    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, azureServiceTokenProvider, new AzureKeyVaultConfigurationOptions());
//}

builder.Services.AddDbContextFactory<ApplicationDbContext>(lifetime: ServiceLifetime.Scoped);

builder.Services.AddSingleton<MockDataService>();
builder.Services.AddSingleton<IElasticSearchService,ElasticSearchService>();

builder.Services.AddSingleton<MultiTenancyMiddlewareBackgroundService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(provider =>
{
    var middlewareFactories = new List<Func<IBackgroundServiceMiddleware>>
    {        
        () => provider.GetRequiredService<MultiTenancyMiddlewareBackgroundService>()
    };

    return new MiddlewarePipeline(middlewareFactories);
});

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSignalR();  // Register SignalR services

string corsOrigin = configuration["CorsOrigin"] ?? string.Empty;
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    policy
    //.SetIsOriginAllowedToAllowWildcardSubdomains()
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    //.WithOrigins(corsOrigin.Split(',')).AllowCredentials()
    );
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<MemberBasicValidator>();
//builder.Services.AddFluentValidationAutoValidation();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMultiTenancy();
app.UseCors("CorsPolicy");

app.MapControllers();

app.MapHub<NotificationHub>("/notificationhub");

app.Run();

// Ensure to flush and stop internal timers/threads before application exits
Log.CloseAndFlush();