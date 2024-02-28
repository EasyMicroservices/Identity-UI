using EasyMicroservices.Security;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using EasyMicroservices.UI.Identity.Models;
using Identity.GeneratedServices;
using System.Net.Http.Json;
using System.Text;

namespace EasyMicroservices.UI.Identity.ViewModels.Authentications
{
    public class LoginViewModel : PageBaseViewModel
    {
        public static string PasswordSalt { get; set; } = "31927acb-9489-46c4-aba9-7f4c7c1d82e1";
        public static string CurrentDomain { get; set; }
        public static string WhiteLabelKey { get; set; }
        public static Func<string, Task> OnGetToken { get; set; }

        public LoginViewModel(AuthenticationClient authenticationClient, HttpClient httpClient, ISecurityProvider securityProvider)
        {
            _httpClient = httpClient;
            _securityProvider = securityProvider;
            _authenticationClient = authenticationClient;
            LoginCommand = new TaskRelayCommand(this, Login);
            RegisterCommand = new TaskRelayCommand(this, Register);

            Clear();
            _ = Load();
        }

        public Func<bool, string, Task> OnLoginFunc { get; set; }


        public bool IsForgotPasswordAvailable { get; set; } = false;
        public TaskRelayCommand LoginCommand { get; set; }
        public TaskRelayCommand RegisterCommand { get; set; }

        readonly AuthenticationClient _authenticationClient;
        readonly HttpClient _httpClient;
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

        public virtual string ComputePassword()
        {
            return _securityProvider.ComputeHexString(PasswordSalt + UserName + Password);
        }

        public async Task Login()
        {
            if (UserName.IsNullOrEmpty() || UserName.Length < 3)
                await DisplayError(GetTranslatedByKey("Username_Validation_ErrorMessage"));
            else if (Password.IsNullOrEmpty() || Password.Length < 7)
                await DisplayError(GetTranslatedByKey("Password_Validation_ErrorMessage"));
            else
            {
                var computedPassword = ComputePassword();
                Console.WriteLine($"ComputedPassword: {computedPassword}");
                _httpClient.DefaultRequestHeaders.Authorization = null;
                var loginResult = await _authenticationClient.LoginAsync(new UserSummaryContract()
                {
                    UserName = UserName,
                    Password = computedPassword,
                    WhiteLabelKey = WhiteLabelKey
                }).AsCheckedResult(x => x.Result);
                OnGetToken?.Invoke(loginResult.Token);
                await OnLoggedIn(true, loginResult.Token);
            }
        }

        public virtual void NavigateToForgotPasswordPage()
        {
            throw new NotImplementedException();
        }

        public virtual async Task OnLoggedIn(bool isLogin, string token)
        {
            if (OnLoginFunc != null)
                await OnLoginFunc(isLogin, token);
        }

        public override async Task OnServerError(ServiceContracts.ErrorContract errorContract)
        {
            await OnLoggedIn(false, null);
            await base.OnServerError(errorContract);
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
