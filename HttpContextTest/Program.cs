using HttpContextTest.Components;
using MudBlazor.Services;

namespace HttpContextTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddMudServices();

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Enable session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true; // Make session cookie HTTP only
                options.Cookie.IsEssential = true; // Make the session cookie essential
            });

            // Register IHttpContextAccessor
            builder.Services.AddHttpContextAccessor(); // Use the built-in method

            builder.Services.AddScoped<SessionService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Use session middleware
            app.UseSession();

            // Other middleware
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.Use(async delegate (HttpContext Context, Func<Task> Next)
            {
                //this throwaway session variable will "prime" the SetString() method
                //to allow it to be called after the response has started
                var TempKey = Guid.NewGuid().ToString(); //create a random key
                Context.Session.Set(TempKey, Array.Empty<byte>()); //set the throwaway session variable
                Context.Session.Remove(TempKey); //remove the throwaway session variable
                await Next(); //continue on with the request
            });

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
