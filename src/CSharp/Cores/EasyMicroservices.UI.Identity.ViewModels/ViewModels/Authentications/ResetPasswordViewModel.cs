using EasyMicroservices.Security;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using EasyMicroservices.UI.Identity.Models;
using Identity.GeneratedServices;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace EasyMicroservices.UI.Identity.ViewModels.Authentications
{
    public class ResetPasswordViewModel : PageBaseViewModel
    {
        public ResetPasswordViewModel(ResetPasswordClient resetPasswordClient, ISecurityProvider securityProvider)
        {
            _resetPasswordClient = resetPasswordClient;
            _securityProvider = securityProvider;

            ResetPasswordCommand = new TaskRelayCommand(this, ResetPassword);
            ValidateCommand = new TaskRelayCommand(this, ValidateTokenAsync);

            Clear();
        }
        public static string PasswordSalt { get; set; } = "31927acb-9489-46c4-aba9-7f4c7c1d82e1";

        readonly ResetPasswordClient _resetPasswordClient;
        readonly ISecurityProvider _securityProvider;

        public TaskRelayCommand ResetPasswordCommand { get; set; }
        public TaskRelayCommand ValidateCommand { get; set; }
        public Action OnSuccess { get; set; }
        public Action OnError { get; set; }

        string _Token;
        public string Token
        {
            get => _Token;
            set
            {
                _Token = value;
                OnPropertyChanged(nameof(Token));
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

        public virtual async Task ValidateTokenAsync()
        {
            if (Token.IsNullOrEmpty())
            {
                OnError?.Invoke();
                await DisplayError(GetTranslatedByKey("Token_Validation_ErrorMessage"));
            }
            else
            {
                var validateResult = await _resetPasswordClient.ValidateResetPasswordTokenAsync(new ValidateResetPasswordTokenRequestContract { Token = Token });
                if(!validateResult.IsSuccess)
                {
                    OnError?.Invoke();
                    await DisplayError(GetTranslatedByKey("Token_NotValid_ErrorMessage"));
                }

                UserName = validateResult.Result.UserName;
            }

        }

        public virtual async Task ResetPassword()
        {
            if (Password.IsNullOrEmpty() || Password.Length < 7)
                await DisplayError(GetTranslatedByKey("Password_Validation_ErrorMessage"));
            else
            {
                var consumeResult = await _resetPasswordClient.ConsumeResetPasswordTokenAsync(new ConsumeResetPasswordTokenRequestContract { Token = Token,  Password = ComputePassword()});
                if(!consumeResult.IsSuccess)
                    OnError?.Invoke();
                else
                    OnSuccess?.Invoke();

                Clear();
            }

        }
        public virtual string ComputePassword()
        {
            return _securityProvider.ComputeHexString(PasswordSalt + UserName + Password);
        }

        public void Clear()
        {
            Token = default;
            Password = default;
        }
    }
}
