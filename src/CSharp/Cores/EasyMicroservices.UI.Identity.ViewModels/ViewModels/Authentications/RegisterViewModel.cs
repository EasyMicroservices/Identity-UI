using EasyMicroservices.Security;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;

namespace EasyMicroservices.UI.Identity.ViewModels.Authentications;

public class RegisterViewModel : ApiBaseViewModel
{
    public RegisterViewModel(global::Identity.GeneratedServices.AuthenticationClient authenticationClient, ISecurityProvider securityProvider)
    {
        _securityProvider = securityProvider;
        _authenticationClient = authenticationClient;
        RegisterCommand = new TaskRelayCommand(this, Register);
        CancelCommand = new TaskRelayCommand(this, Cancel);
        Clear();
    }

    public TaskRelayCommand RegisterCommand { get; set; }
    public TaskRelayCommand CancelCommand { get; set; }

    readonly global::Identity.GeneratedServices.AuthenticationClient _authenticationClient;
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

    string _ConfirmPassword;
    public string ConfirmPassword
    {
        get => _ConfirmPassword;
        set
        {
            _ConfirmPassword = value;
            OnPropertyChanged(nameof(ConfirmPassword));
        }
    }

    public virtual async Task<MessageContract<global::Identity.GeneratedServices.RegisterResponseContract>> Register()
    {
        if (Password != ConfirmPassword)
        {
            await DisplayError(GetLanguage("Password_Not_Match"));
            return FailedReasonType.ValidationsError;
        }

        var loginResult = await _authenticationClient.RegisterAsync(new()
        {
            UserName = UserName,
            Password = _securityProvider.ComputeHexString(Password),
            WhiteLabelKey = LoginViewModel.WhiteLabelKey
        }).AsCheckedResult(x => x.Result);
        return loginResult;
    }

    public void Clear()
    {
        UserName = default;
        Password = default;
    }

    public virtual Task Cancel()
    {
        return Task.CompletedTask;
    }
}