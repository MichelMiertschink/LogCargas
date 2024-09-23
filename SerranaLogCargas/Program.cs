using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LogCargas.Data;
using LogCargas.Services;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using ReflectionIT.Mvc.Paging;
using LogCargas.Mappings;
using LogCargas.Interfaces;
using LogCargas.REST;
using System.Text.Json.Serialization;

namespace LogCargas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContextPool<LogCargasContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Registrando injecao de dependencia para os servi�os
            builder.Services.AddScoped<SeedingService>();
            builder.Services.AddScoped<CityService>();
            builder.Services.AddScoped<StateService>();
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<VehicleService>();
            builder.Services.AddScoped<DriverService>();
            builder.Services.AddScoped<LoadSchedulingService>();
            builder.Services.AddScoped<RedeFrotaService>();
            builder.Services.AddScoped<IRedeFrotaService, RedeFrotaService>();
            builder.Services.AddScoped<IRedeFrotaApi, RedeFrotaApiRest>();
            builder.Services.AddAutoMapper(typeof(RedeFrotaMapping));
           // builder.Services.AddScoped<RedeFrotaService>();

           // builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Seeding service
            var conectionString = builder.Configuration.GetConnectionString("AppDb");
            builder.Services.AddTransient<SeedingService>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LogCargasContext>();

            builder.Services.AddRazorPages();


            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            });

            builder.Services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // Paginação
            builder.Services.AddPaging(options =>
            {
                options.ViewName = "Bootstrap4";
                options.PageParameterName = "pageindex";
            });

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //SeedingService seedingService = new SeedingService(context);

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}