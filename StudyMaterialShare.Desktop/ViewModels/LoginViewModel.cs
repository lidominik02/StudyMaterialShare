using StudyMaterialShare.Desktop.ViewModel;
using StudyMaterialShare.Data;
using StudyMaterialShare.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;

namespace StudyMaterialShare.Desktop.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _userName = string.Empty;
        private bool _isNotLoading = true;
        private readonly IStudyMaterialShareApiService _service;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        public bool IsNotLoading
        {
            get => _isNotLoading;
            set
            {
                _isNotLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; set; }

        public event Action? LoginSucceeded;

        public LoginViewModel(IStudyMaterialShareApiService service)
        {
            LoginCommand = new DelegateCommand(CanLoginCommandExecute,param => OnLogin(param as PasswordBox));
            _service = service;
        }

        private bool CanLoginCommandExecute(object? obj)
        {
            return !string.IsNullOrEmpty(UserName) && IsNotLoading;
        }

        private async void OnLogin(PasswordBox? pb)
        {
            try
            {
                if (pb is null) return;

                IsNotLoading = false;
                await _service.LoginAsync(new LoginDTO()
                {
                    UserName = UserName,
                    Password = pb.Password
                });
                IsNotLoading = true;

                OnLoginSucceed();
            }
            catch (NetworkException netEx)
            {
                if(netEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    OnMessageApplication("Rossz felhasználó név vagy jelszó!");
                }
                else if (netEx.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    OnMessageApplication("Nincs admin jogosultságod!");
                }
                else
                {
                    OnMessageApplication(netEx.Message);
                }
            }
            catch (Exception e)
            {
                OnMessageApplication(e.Message);
            }
            finally
            {
                if(pb is not null)
                    pb.Password = null;
                IsNotLoading = true;
            }
        }

        private void OnLoginSucceed()
        {
            LoginSucceeded?.Invoke();
        }
    }
}
