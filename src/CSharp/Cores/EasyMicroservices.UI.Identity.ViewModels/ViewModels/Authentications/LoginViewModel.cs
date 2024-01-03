﻿using EasyMicroservices.Security;
using EasyMicroservices.Security.Interfaces;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using EasyMicroservices.UI.Identity.Models;
using Identity.GeneratedServices;
using System.Net.Http.Json;

namespace EasyMicroservices.UI.Identity.ViewModels.Authentications
{
    public class LoginViewModel : ApiBaseViewModel
    {
        public static string CurrentDomain { get; set; }
        public static string WhiteLabelKey { get; set; }
        public static Func<string,Task> OnGetToken { get; set; }

        public LoginViewModel(AuthenticationClient authenticationClient, ISecurityProvider securityProvider)
        {
            _securityProvider = securityProvider;
            _authenticationClient = authenticationClient;
            LoginCommand = new TaskRelayCommand(this, Login);
            RegisterCommand = new TaskRelayCommand(this, Register);
            
            Clear();
            _ = Load();
        }

        public Action<bool> OnLogin { get; set; }

        public TaskRelayCommand LoginCommand { get; set; }
        public TaskRelayCommand RegisterCommand { get; set; }

        readonly AuthenticationClient _authenticationClient;
        readonly ISecurityProvider _securityProvider;
        string _UserName;
        public string UserName
        {
            get => _UserName;
            set
            {
                _UserName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        string _Password;
        public string Password
        {
            get => _Password;
            set
            {
                _Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public async Task Login()
        {
            var loginResult = await  _authenticationClient.LoginAsync(new UserSummaryContract()
            {
                UserName = UserName,
                Password = _securityProvider.ComputeHexString(Password),
                WhiteLabelKey = WhiteLabelKey
            }).AsCheckedResult(x => x.Result);
            OnGetToken?.Invoke(loginResult.Token);
            OnLogin?.Invoke(true);
        }

        public override Task OnServerError(ServiceContracts.ErrorContract errorContract)
        {
            OnLogin?.Invoke(false);
            return base.OnServerError(errorContract);
        }

        public virtual async Task Load()
        {
            try
            {
                IsBusy = true;
                var config = await new HttpClient().GetFromJsonAsync<ApplicationConfig>($"{CurrentDomain}/appsettings.json");
                WhiteLabelKey = config.WhiteLabels.FirstOrDefault(x => x.Domain.Equals(new Uri(CurrentDomain).Authority, StringComparison.OrdinalIgnoreCase))?.Key;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public virtual Task Register()
        {
            return Task.CompletedTask;
        }

        public void Clear()
        {
            UserName = default;
            Password = default;
        }
    }
}
