using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MTC.OneAPIv2.Model;
using MTC.OneAPIv2.Services;
using System;

namespace MTC.OneAPIv2
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
            services.AddDbContext<MTCContext>(opt => opt.UseInMemoryDatabase("MTC"));

            services.Configure<MTCDatabaseSettings>(Configuration.GetSection(nameof(MTCDatabaseSettings)));
            services.AddSingleton<IMTCDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MTCDatabaseSettings>>().Value);
            services.AddSingleton<MTCLocationsService>(); // Per the official Mongo Client reuse guidelines, MongoClient should be registered in DI with a singleton service lifetime.


            services.AddSingleton<IMTCLocationRepository, MTCLocationRepository>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v3", new OpenApiInfo
                {
                    Version = "v3",
                    Title = "One MTC API",
                    Description = "",
                    TermsOfService = new Uri(""),
                    Contact = new OpenApiContact
                    {
                        Name = "Mikhail Bondarevsky",
                        Email = "mikhail@mikhail.com",
                        Url = new Uri("https://twitter.com/test")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Unser under LICX",
                        Url = new Uri("https://example.com/license")
                    }
                });
            });

            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing()); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My OneMTCOpenAPI V3");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
