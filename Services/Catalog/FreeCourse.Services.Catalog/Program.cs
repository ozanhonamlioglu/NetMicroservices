using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace FreeCourse.Services.Catalog
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
        var sp = scope.ServiceProvider;

        var categoryService = sp.GetRequiredService<ICategoryService>();

        if (!categoryService.GetAll().Result.Data.Any())
        {
          categoryService.CreateAsync(new CategoryCreateDto { Name = "Asp.net Core Kursu" }).Wait();
          categoryService.CreateAsync(new CategoryCreateDto { Name = "Asp.net Core Api Kursu" }).Wait();
        }

      }

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
