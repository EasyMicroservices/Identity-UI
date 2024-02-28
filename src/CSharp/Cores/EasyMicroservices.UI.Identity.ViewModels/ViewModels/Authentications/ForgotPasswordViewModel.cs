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
    public class ForgotPasswordViewModel : PageBaseViewModel
    {
        public ForgotPasswordViewModel()
        {
            ForgotPasswordCommand = new TaskRelayCommand(this, ForgotPassword);

            Clear();
        }


        public TaskRelayCommand ForgotPasswordCommand { get; set; }
        public Action OnSuccess { get; set; }

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

        public virtual async Task ForgotPassword()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            UserName = default;
        }
    }
}
