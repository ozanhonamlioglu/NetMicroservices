using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog
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
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddScoped<ICourseService, CourseService>();

      services.AddAutoMapper(typeof(Startup));
      services.AddControllers(options =>
      {
        options.Filters.Add(new AuthorizeFilter());
      });

      // herhangi bir class içeirisinde IOptisons<DatabaseSettings> yazarak, appsettings bilgilerine ulaşabiliriz Options pattern ile
      services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));

      // Aşağıdaki singleton'ı ise Options pattern yerine interface ile ulaşmak için yaptık, yani appsettings'e ulaşmak istersem
      // private readonly IDatabaseSettings _databaseSettings; demem yeterli olacak.
      services.AddSingleton<IDatabaseSettings>(sp =>
      {
        return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Catalog", Version = "v1" });
      });


      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
      {
        opts.Authority = Configuration["IdentityServerURL"];
        opts.Audience = "resource_catalog";
        opts.RequireHttpsMetadata = false;
      });

      services.AddAuthorization(opts =>
      {
        opts.AddPolicy("RequireAdmin", policy =>
        {
          policy.RequireRole("admin");
        });
      });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Catalog v1"));
      }

      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
