using EasyMicroservices.FileManager.Interfaces;
using EasyMicroservices.FileManager.Providers.DirectoryProviders;
using EasyMicroservices.FileManager.Providers.FileProviders;
using EasyMicroservices.FileManager.Providers.PathProviders;
using EasyMicroservices.Security;
using EasyMicroservices.Security.Providers.HashProviders;
using EasyMicroservices.UI.Identity.Blazor.TestUI;
using EasyMicroservices.UI.Identity.Models;
using EasyMicroservices.UI.Identity.ViewModels.Authentications;
using Identity.GeneratedServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new AuthenticationClient("http://localhost:2007", sp.GetService<HttpClient>()));
builder.Services.AddScoped(sp => new ResetPasswordClient("http://localhost:2007", sp.GetService<HttpClient>()));
builder.Services.AddScoped<ISecurityProvider, SHA256HashProvider>();

builder.Services.AddTransient<LoginViewModel>();
builder.Services.AddScoped<ForgotPasswordViewModel, ForgotPasswordChildViewModel>(); 
builder.Services.AddScoped<ResetPasswordViewModel>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});
await LoadAppSettingsAsync(builder);
LoginViewModel.CurrentDomain = builder.HostEnvironment.BaseAddress;
var builderHost = builder.Build();

//Stream stream = Assembly.GetExecutingAssembly()
//                       .GetManifestResourceStream("EasyMicroservices.UI.Identity.appsettings.json");
//var configuration = new ConfigurationBuilder()
//    .AddJsonStream(stream)
//    .Build();

var whiteLabels = builderHost.Configuration.GetSection("WhiteLabels").Get<List<WhiteLabelConfig>>();

await builderHost.RunAsync();
static async Task LoadAppSettingsAsync(WebAssemblyHostBuilder builder)
{
    var client = new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    using var response = await client.GetAsync("appsettings.json");
    using var stream = await response.Content.ReadAsStreamAsync();
    builder.Configuration.AddJsonStream(stream);
}