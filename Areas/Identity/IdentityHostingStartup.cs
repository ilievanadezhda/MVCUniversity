using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MVCUniversity.Areas.Identity.Data;
using MVCUniversity.Data;

[assembly: HostingStartup(typeof(MVCUniversity.Areas.Identity.IdentityHostingStartup))]
namespace MVCUniversity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MVCUniversityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MVCUniversityContext")));
            });
        }
    }
}