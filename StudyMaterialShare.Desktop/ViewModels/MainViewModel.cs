using Microsoft.Extensions.DependencyInjection;
using StudyMaterialShare.Desktop.ViewModel;
using StudyMaterialShare.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;

namespace StudyMaterialShare.Desktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IStudyMaterialShareApiService _service;

        public BrowseViewModel BrowseViewModel { get; set; } = null!;
        public SubjectTabViewModel SubjectTabViewModel { get; set; }

        public event EventHandler LogoutEvent = null!;

        public ICommand LogoutCommand { get; set; }

        public MainViewModel(IServiceProvider serviceProvider,IStudyMaterialShareApiService service)
        {
            BrowseViewModel = serviceProvider.GetRequiredService<BrowseViewModel>();
            SubjectTabViewModel = serviceProvider.GetRequiredService<SubjectTabViewModel>();

            SubjectTabViewModel.SubjectAddedEvent += SubjectTabViewModel_SubjectAddedEvent;
            SubjectTabViewModel.SubjectDeletedEvent += SubjectTabViewModel_SubjectDeletedEvent;
            SubjectTabViewModel.SubjectModifiedEvent += SubjectTabViewModel_SubjectModifiedEvent;

            SubjectTabViewModel.MessageApplication += ViewModel_MessageApplication;
            BrowseViewModel.MessageApplication += ViewModel_MessageApplication;

            LogoutCommand = new DelegateCommand(_ => OnLogout());

            _service = service;
        }

        private void SubjectTabViewModel_SubjectModifiedEvent(object? sender, SubjectViewModel e)
        {
            var modifiedSubject = BrowseViewModel.Subjects.FirstOrDefault(s => s.Id == e.Id);

            if(modifiedSubject != null)
                modifiedSubject.Name = e.Name;
        }

        private void SubjectTabViewModel_SubjectDeletedEvent(object? sender, SubjectViewModel e)
        {
            var subject = BrowseViewModel.Subjects.FirstOrDefault(s => s.Id == e.Id);

            if(subject != null)
                BrowseViewModel.Subjects.Remove(subject);
        }

        private void SubjectTabViewModel_SubjectAddedEvent(object? sender, SubjectViewModel e)
        {
            BrowseViewModel.Subjects.Add(e);
        }

        private void ViewModel_MessageApplication(object? sender, string e)
        {
            OnMessageApplication(e);
        }

        private async void OnLogout()
        {
            await _service.LogoutAsync();

            OnLogoutEvent();
        }

        private void OnLogoutEvent()
        {
            LogoutEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
