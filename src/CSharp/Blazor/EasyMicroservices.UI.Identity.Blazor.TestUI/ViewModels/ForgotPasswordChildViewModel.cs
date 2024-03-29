﻿using EasyMicroservices.Security;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using EasyMicroservices.UI.Identity.Models;
using EasyMicroservices.UI.Identity.ViewModels.Authentications;
using Identity.GeneratedServices;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace EasyMicroservices.UI.Identity.Blazor.TestUI.ViewModels
{
    public class ForgotPasswordChildViewModel : ForgotPasswordViewModel
    {
        public override Task ForgotPassword()
        {
            Console.WriteLine("Lalala");
            return Task.CompletedTask;
        }
    }
}
