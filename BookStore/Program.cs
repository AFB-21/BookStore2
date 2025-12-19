using BookStore.Api.Middleware;
using BookStore.Application.Behaviors;
using BookStore.Application.Data;
using BookStore.Application.Features.Authors.Queries.Validators;
using BookStore.Application.Features.Books.Commands.Models;
using BookStore.Application.Features.Books.Commands.Validators;
using BookStore.Application.Interfaces;
using BookStore.Application.Mapping;
using BookStore.Infrastructure.Bases;
using BookStore.Infrastructure.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;


// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/bookstore-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting BookStore API");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        // JWT Bearer support in Swagger (use Http/bearer for proper OpenAPI semantics)
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your token. Example: \"Bearer eyJhbGci...\""
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
        });
    });

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SecondaryConnection")));


    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<MappingProfile>(); // أو جميع الـ Profiles كما تريد
    });
    builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        // other options...
    })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();


    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var secret = jwtSettings["Key"];

    // Validate secret presence/strength at startup
    if (string.IsNullOrWhiteSpace(secret) || Encoding.UTF8.GetByteCount(secret) < 32)
    {
        throw new System.InvalidOperationException("JWT key is missing or too short. Provide a key at least 32 characters long in configuration.");
    }


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Enforce HTTPS metadata, save token and remove default clock skew for strict expiry checks
        options.RequireHttpsMetadata = false;
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("JWT validation failed: {error}", context.Exception.Message);
                return Task.CompletedTask;
            }
        };
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = System.TimeSpan.Zero
        };
    });

    builder.Services.AddRateLimiter(options =>
    {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                factory: partition => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1)
                }));
    });


    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateBookCommand).Assembly));
    builder.Services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<UpdateBookCommandValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<GetAuthorQueryValidators>();
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp", policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });


    builder.Services.AddHealthChecks()
        .AddDbContextCheck<AppDbContext>(
            name: "database",
            tags: new[] { "db", "sql" })
        .AddCheck("self", () =>
            Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API is running"),
            tags: new[] { "api" });

    var app = builder.Build();

    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseRateLimiter();
    app.UseHttpsRedirection();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("AllowReactApp");

    app.MapHealthChecks("/health");
    app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("db")
    });
    app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = _ => false // Only check if API is running (no dependencies)
    });

    app.MapControllers();

    //seed roles and users
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var db = services.GetRequiredService<AppDbContext>();
            if (db.Database.CanConnect())
            {
                db.Database.Migrate();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await IdentityDataSeeder.SeedRolesAndUsersAsync(userManager, roleManager, builder.Configuration, logger);
            }
            else
            {
                logger.LogWarning("Database is not reachable. Skipping migrations and seeding.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database migration/seed failed. Application will continue without applying migrations.");
            // decide whether to rethrow in production
        }
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}