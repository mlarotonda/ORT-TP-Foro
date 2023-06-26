using Foro_C.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Foro_C
{
    public static class StartUp
    {
        public static WebApplication InicializarApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();
            Configure(app);

            return app;
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ForoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ForoDBCS")));

            builder.Services.AddIdentity<Models.Persona, Models.Rol>().AddEntityFrameworkStores<ForoContext>();

            //builder.Services.Configure<ForoContext>(options => options.)

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
            {
                opciones.LoginPath = "/Account/Iniciarsesion";
                opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                opciones.Cookie.Name = "IdentityCookie";


            });



            builder.Services.AddControllersWithViews();
        }

        public static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            using(var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contexto = serviceScope.ServiceProvider.GetRequiredService<ForoContext>();

                contexto.Database.Migrate();

            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

    }
}