using EasyMicroservices.UI.Identity.Helpers;
using EasyMicroservices.UI.Identity.ViewModels.Authentications;
using Identity.GeneratedServices;
using Microsoft.Extensions.Logging;

namespace EasyMicroservices.UI.Identity.Maui.TestUI;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddScoped(sp => new HttpClient());
        builder.Services.AddScoped(sp => new AuthenticationClient("http://localhost:2007", sp.GetService<HttpClient>()));
        builder.Services.AddScoped<LoginViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var build = builder.Build();
        ViewModelLocator.ServiceProvider = build.Services;
        return build;
    }
}
