using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhoIsFaster.Infrastructure.Data.Persistance;
using WhoIsFaster.Infrastructure.Data.Persistance.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WhoIsFaster.Domain.Interfaces;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.ApplicationServices.Services;
using WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagers;
using WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagerInterfaces;
using WhoIsFaster.BlazorApp.BackgroundServices;
using WhoIsFaster.Infrastructure.SignalRNotifications.Hubs;
using WhoIsFaster.BlazorApp.EmailServices;
using WhoIsFaster.BlazorApp.GameServices;
using WhoIsFaster.BlazorApp.EventServices;

namespace WhoIsFaster.BlazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WhoIsFasterDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders()
                    .AddEntityFrameworkStores<WhoIsFasterDbContext>();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRegularUserService, RegularUserService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ITextService, TextService>();
            services.AddScoped<IEventService, EventService>();	
            
            services.AddSingleton<IGameNotificationManager, GameNotificationManager>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddHostedService<GameLoopService>();
            services.AddSignalR();

            services.AddHttpContextAccessor();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapHub<WhoIsFasterSignalRHub>("/whoIsFasterSignalRHub");
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapRazorPages();
            });


            WhoIsFasterDbContext.CreateRoles(app.ApplicationServices, Configuration).Wait();
            WhoIsFasterDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
        }
    }
}
