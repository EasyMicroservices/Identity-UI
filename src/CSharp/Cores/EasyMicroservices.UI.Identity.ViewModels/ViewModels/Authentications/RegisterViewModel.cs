using EasyMicroservices.Security;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using System.Text;

namespace EasyMicroservices.UI.Identity.ViewModels.Authentications;

public class RegisterViewModel : PageBaseViewModel
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

    public virtual string ComputePassword()
    {
        return _securityProvider.ComputeHexString(LoginViewModel.PasswordSalt + UserName + Password);
    }

    public virtual async Task<MessageContract<global::Identity.GeneratedServices.RegisterResponseContract>> Register()
    {
        if (UserName.IsNullOrEmpty() || UserName.Length < 3)
            return (FailedReasonType.ValidationsError, "Username_Validation_ErrorMessage");
        else if (Password.IsNullOrEmpty() || Password.Length < 7)
            return (FailedReasonType.ValidationsError, "Password_Validation_ErrorMessage");
        else if (Password != ConfirmPassword)
            return (FailedReasonType.ValidationsError, "Password_Not_Match");
        else
        {
            var loginResult = await _authenticationClient.RegisterAsync(new()
            {
                UserName = UserName,
                Password = ComputePassword(),
                WhiteLabelKey = LoginViewModel.WhiteLabelKey
            }).AsCheckedResult(x => x.Result);
            return loginResult;
        }
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