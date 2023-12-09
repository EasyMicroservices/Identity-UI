using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using Identity.GeneratedServices;

namespace EasyMicroservices.UI.Identity.ViewModels.Authentications
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(AuthenticationClient authenticationClient)
        {
            _authenticationClient = authenticationClient;
            LoginCommand = new TaskRelayCommand(this, Login);
            Clear();
            OnBusyChanged = (b) =>
            {
                OnPropertyChanged(nameof(IsNotBusy));
            };
        }

        public Action<string> OnSuccess { get; set; }

        public TaskRelayCommand LoginCommand { get; set; }

        readonly AuthenticationClient _authenticationClient;

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

        public bool IsNotBusy
        {
            get
            {
                return !IsBusy;
            }
        }

        public async Task Login()
        {
            await ExecuteApi(async () =>
            {
                return await _authenticationClient.LoginAsync(new UserSummaryContract()
                {
                    UserName = UserName,
                    Password = Password
                });
            }, (LoginWithTokenResponseContract result) =>
            {
                OnSuccess(result.Token);
                return Task.CompletedTask;
            });
        }


        public override Task OnError(Exception exception)
        {
            return base.OnError(exception);
        }

        public override Task DisplayFetchError(ServiceContracts.ErrorContract errorContract)
        {
            return base.DisplayFetchError(errorContract);
        }

        public void Clear()
        {
            UserName = default;
            Password = default;
        }
    }
}
