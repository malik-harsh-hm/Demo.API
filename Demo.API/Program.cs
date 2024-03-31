
using Asp.Versioning;
using Demo.API.Data;
using Demo.API.Models;
using Demo.API.Repository;
using Demo.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Demo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Entity Framework
            builder.Services.AddDbContext<AppDbContext>(o =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DemoAPIConnectionString");
                o.UseSqlServer(connectionString);
            });

            // Identity needs to be added before authentication.
            // Identity Fraework
            builder.Services.AddIdentity<UserModel, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            // JWT authentication
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                    };
                });
            // Repositories
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddTransient<IAccountRepository, AccountRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DI
            builder.Services.AddScoped<IFatherService, FatherService>();
            builder.Services.AddScoped<IMotherService, MotherService>();
            builder.Services.AddSingleton<IChildService, ChildService>();

            // Api Versioning
            //var apiVersionBuilder = builder.Services.AddApiVersioning(o =>
            //{
            //    o.AssumeDefaultVersionWhenUnspecified = true;
            //    o.DefaultApiVersion = new ApiVersion(1, 0);        // Default version 1.0 if a client doesn’t specify the version of the API
            //    o.ReportApiVersions = true;                        // Adds 'api-supported-versions' and 'api-deprecated-versions' response headers
            //    o.ApiVersionReader = ApiVersionReader.Combine(
            //        new QueryStringApiVersionReader("api-version"),// Read version from a query string 'api-version'
            //        new HeaderApiVersionReader("X-Version")        // Read version from a request header 'X-Version'
            //        );
            //});
            //apiVersionBuilder.AddApiExplorer(o =>
            //{
            //    o.GroupNameFormat = "'v'VVV";                      // Will format the version as “‘v’major[.minor][-status]”
            //    o.SubstituteApiVersionInUrl = true;                // Necessary when versioning by the URL segment
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // Custom Middleware
            app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
            {
                builder.Use(async (context, next) =>
                {
                    Console.WriteLine("Branch: Before logic");
                    await next.Invoke();
                    Console.WriteLine("Branch: After logic");
                });
                builder.Run(async context =>
                {
                    Console.WriteLine($"Branch: Terminal middleware");
                    await context.Response.WriteAsync("Hello from the Map branch");
                });
            });
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Before logic");
                await next.Invoke();
                Console.WriteLine($"After logic");
            });
            //app.Run(async context =>
            //{
            //    Console.WriteLine($"Terminal middleware");
            //    await context.Response.WriteAsync("Hello from the Run delegate");
            //});

            // Enable Attribute Routing
            app.MapControllers();

            app.Run();
        }
    }
}
