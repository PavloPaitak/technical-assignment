using TechnicalAssignment.Application.Services;
using TechnicalAssignment.Components;
using TechnicalAssignment.Infrastructure.Extensions;
using TechnicalAssignment.Infrastructure.Services;

namespace TechnicalAssignment;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;

        // Add services to the container
        builder.Services.AddItemApi(config);
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<IItemService, ItemService>();
        builder.Services.Configure<ItemServiceSettings>(builder.Configuration.GetSection(ItemServiceSettings.SectionName));
        builder.Services.AddScoped<IImageCacheService, ImageCacheService>();
        builder.Services.AddHttpClient<ImageCacheService>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .ConfigureHttpClient(c => c.Timeout = TimeSpan.FromSeconds(10)); // todo: pp can be moved to appsettings
        builder.Services.AddControllers();
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        // Build app
        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAntiforgery();
        app.MapControllers();
        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        // Run app
        app.Run();
    }
}
