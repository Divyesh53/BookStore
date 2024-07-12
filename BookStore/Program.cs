// Early init of NLog to allow startup and exception logging, before host is built
using BookStore.Models.Models;
using BookStore.Repository.Interface;
using BookStore.Repository.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Text;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Initialized Logger for BookStoreAPI");


try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();

    // connecton setting //
    var connectionString = builder.Configuration.GetConnectionString("BookStoreDb");
    builder.Services.AddDbContext<BookStoreDBContext>(x => x.UseSqlServer(connectionString));

    //Add Services
    builder.Services.AddScoped<IAuthService, AuhService>();
    builder.Services.AddScoped<IBooksService, BooksService>();
    builder.Services.AddScoped<ICategoriesService, CategoriesService>();
    builder.Services.AddScoped<IPurchasesService, PurchasesService>();
    builder.Services.AddScoped<IRolesService, RolesService>();
    builder.Services.AddScoped<IUsesrService, UsersService>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddAuthentication(opt =>
    {
        // Configure default authentication scheme to use JWT Bearer authentication
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(option =>
    {
        option.RequireHttpsMetadata = false;
        option.SaveToken = true;
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:SecretKey"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
        };
    });

    // Add authorization policies
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
        options.AddPolicy("Buyer", policy => policy.RequireRole("Buyer"));
        options.AddPolicy("Seller", policy => policy.RequireClaim("Seller"));
    });

    builder.Services.AddSwaggerGen(c =>
    {
        //Configure Swagger document
        c.SwaggerDoc("v1", new OpenApiInfo()
        {
            Title = "JWT Auth",
            Version = "v1"
        });

        //Configure swagger to use JWT bearer authorization
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWt",
            In = ParameterLocation.Header,
            Description = "Enter token after in this format 'Bearer [space] [token].'",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

    // Logger initiated for error logging withing application
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Program -> Stopped Program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
