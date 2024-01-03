using EasyMicroservices.UI.Identity.ViewModels.Authentications;
using Microsoft.Extensions.DependencyInjection;

namespace EasyMicroservices.UI.Identity.Helpers;
public class ViewModelLocator
{
    public static IServiceProvider ServiceProvider { get; set; }
    public LoginViewModel LoginViewModel
    {
        get
        {
            return ServiceProvider.GetService<LoginViewModel>();
        }
    }

    public RegisterViewModel RegisterViewModel
    {
        get
        {
            return ServiceProvider.GetService<RegisterViewModel>();
        }
    }
}
