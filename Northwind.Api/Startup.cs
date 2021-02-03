using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Northwind.Common.Configuration;
using Northwind.Common.Interface;
using Northwind.Data.Context;
using Northwind.Data.Repository;
using Northwind.Services;

namespace Northwind.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NorthwindContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("NorthwindDb"));
            });

            services.AddCors();
            //We set the default for all controllers to be authorize
            services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            // configure DI for application services
            // change your testing data provider
            services.AddScoped<ICustomerService, EFCoreCustomerService>();
            //services.AddScoped<ICustomerService, ADONetCustomerService>();
            //services.AddScoped<ICustomerService, MemoryCustomerService>();

            // configure DI for application repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddMvc(opt => opt.EnableEndpointRouting = false);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}