using EasyMicroservices.UI.Identity.ViewModels.Authentications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
