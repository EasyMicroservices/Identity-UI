using EasyMicroservices.Domain.Contracts.Common;
using EasyMicroservices.Security;
using EasyMicroservices.Security.Providers.HashProviders;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Identity.Helpers;
using EasyMicroservices.UI.Identity.ViewModels.Authentications;
using Identity.GeneratedServices;
using Microsoft.Extensions.Logging;

namespace EasyMicroservices.UI.Identity.Maui.TestUI;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        LoadLanguage("en-US");
        BaseViewModel.CurrentApplicationLanguage = "en-US";
        BaseViewModel.IsRightToLeft = false;
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
        builder.Services.AddScoped<ISecurityProvider, SHA256HashProvider>();
        builder.Services.AddScoped<LoginViewModel>();
        builder.Services.AddScoped<RegisterViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var build = builder.Build();
        ViewModelLocator.ServiceProvider = build.Services;
        return build;
    }

    static void LoadLanguage(string languageShortName)
    {
        BaseViewModel.AppendLanguage("Username_Title", new LanguageContract()
        {
            ShortName = languageShortName,
            Value = "Username:"
        });
        BaseViewModel.AppendLanguage("Password_Title", new LanguageContract()
        {
            ShortName = languageShortName,
            Value = "Password:"
        });
        BaseViewModel.AppendLanguage("Login", new LanguageContract()
        {
            ShortName = languageShortName,
            Value = "Login"
        });
        BaseViewModel.AppendLanguage("Cancel", new LanguageContract()
        {
            ShortName = languageShortName,
            Value = "Cancel"
        });
        BaseViewModel.AppendLanguage("Register", new LanguageContract()
        {
            ShortName = languageShortName,
            Value = "Register"
        });
    }
}
