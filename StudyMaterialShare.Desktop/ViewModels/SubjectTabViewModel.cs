using AutoMapper;
using StudyMaterialShare.Data;
using StudyMaterialShare.Desktop.Models;
using StudyMaterialShare.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudyMaterialShare.Desktop.ViewModels
{
    public class SubjectTabViewModel : ViewModelBase
    {
        private readonly IStudyMaterialShareApiService _service;
        private readonly IMapper _mapper;
        private string _newSubjectName = string.Empty;
        private bool _isSubjectEditModalOpen;
        private bool _isEnsureDeleteModalOpen;
        private SubjectViewModel? _selectedSubject;
        private SubjectViewModel? _editableSubject;

        public ObservableCollection<SubjectViewModel> Subjects { get; set; } = null!;

        public ICommand AddSubjectCommand { get; set; }
        public string NewSubjectName {
            get => _newSubjectName;
            set { _newSubjectName = value; OnPropertyChanged(); }
        }

        public bool IsSubjectEditModalOpen
        { 
            get => _isSubjectEditModalOpen; 
            set { _isSubjectEditModalOpen = value; OnPropertyChanged(); }
        }
        public bool IsEnsureDeleteModalOpen { 
            get => _isEnsureDeleteModalOpen; 
            set { _isEnsureDeleteModalOpen = value; OnPropertyChanged(); }
        }
        public SubjectViewModel? SelectedSubject 
        { 
            get => _selectedSubject; 
            set { _selectedSubject = value; OnPropertyChanged(); }
        }
        public SubjectViewModel? EditableSubject 
        { 
            get => _editableSubject; 
            set { _editableSubject = value; OnPropertyChanged(); } 
        }

        public ICommand SubjectEditModalOpenCommand { get; set; }
        public ICommand SubjectEditModalCloseCommand { get; set; }
        public ICommand SubjectEditModalSaveCommand { get; set; }
        public ICommand SubjectDeleteCommand { get; set; }
        public ICommand EnsureDeleteModalOpenCommand { get; set; }
        public ICommand EnsureDeleteModalCloseCommand { get; set; }

        public event EventHandler<SubjectViewModel> SubjectAddedEvent = null!;
        public event EventHandler<SubjectViewModel> SubjectDeletedEvent = null!;
        public event EventHandler<SubjectViewModel> SubjectModifiedEvent = null!;

        public SubjectTabViewModel(IStudyMaterialShareApiService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
            Subjects = new ObservableCollection<SubjectViewModel>();

            AddSubjectCommand = new DelegateCommand(_ => !string.IsNullOrEmpty(NewSubjectName), _ => OnAddSubject());

            SubjectEditModalOpenCommand = new DelegateCommand(OnSubjectEditModelOpen);
            SubjectEditModalCloseCommand = new DelegateCommand(OnSubjectEditModelClose);
            SubjectEditModalSaveCommand = new DelegateCommand(OnSubjectEditModelSave);

            EnsureDeleteModalCloseCommand = new DelegateCommand(OnEnsureDeleteModalClose);
            EnsureDeleteModalOpenCommand = new DelegateCommand(OnEnsureDeleteModalOpen);
            SubjectDeleteCommand = new DelegateCommand(OnSubjectDelete);

            LoadSubjectsAsync();
        }

        private async void OnSubjectDelete(object? obj)
        {
            try
            {
                IsEnsureDeleteModalOpen = false;

                if (SelectedSubject == null) return;
                   
                var deletedSubject = await _service.RemoveSubjectAsync(SelectedSubject.Id);

                SelectedSubject = null;

                LoadSubjectsAsync();
                OnSubjectDeletedEvent(_mapper.Map<SubjectViewModel>(deletedSubject));
            }
            catch (NetworkException netEx)
            {
                if(netEx.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
                {
                    OnMessageApplication("Ezt a tárgyat nem törölheted mivel még vannak hozzá tananyagok!");
                }
                else{
                    OnMessageApplication(netEx.Message);
                }
            }
            catch (Exception e)
            {
                OnMessageApplication(e.Message);
            }
        }

        private void OnEnsureDeleteModalOpen(object? obj)
        {
            SelectedSubject = obj as SubjectViewModel;
            IsEnsureDeleteModalOpen = true;
        }

        private void OnEnsureDeleteModalClose(object? obj)
        {
            SelectedSubject = null;
            IsEnsureDeleteModalOpen = false;
        }

        private void OnSubjectEditModelOpen(object? obj)
        {
            if (obj is null) return;

            SelectedSubject = obj as SubjectViewModel;


            EditableSubject = SelectedSubject?.ShallowClone();
            IsSubjectEditModalOpen = true;
        }

        private async void OnSubjectEditModelSave(object? obj)
        {
            try
            {
                if (EditableSubject == null) return;

                var editedSubject = await _service.UpdateSubjectAsync(_mapper.Map<SubjectDTO>(EditableSubject));

                //SelectedSubject?.CopyFrom(new SubjectViewModel(editedSubject));
                _mapper.Map(editedSubject,SelectedSubject);

                SelectedSubject = null;
                EditableSubject = null;
                IsSubjectEditModalOpen = false;

                LoadSubjectsAsync();
                OnSubjectModifiedEvent(_mapper.Map<SubjectViewModel>(editedSubject));
            }
            catch (Exception e)
            {
                OnMessageApplication(e.Message);
            }
        }

        private void OnSubjectEditModelClose(object? obj)
        {
            IsSubjectEditModalOpen = false;
            EditableSubject = null;
            SelectedSubject = null;
        }

        private async void OnAddSubject()
        {
            try
            {
                var addedSubject = await _service.CreateSubjectAsync(new SubjectDTO()
                {
                    Name = NewSubjectName
                });

                NewSubjectName = string.Empty;

                LoadSubjectsAsync();

                OnSubjectAddedEvent(_mapper.Map<SubjectViewModel>(addedSubject));
            }
            catch (Exception e)
            {
                OnMessageApplication(e.Message);
            }
        }

        private async void LoadSubjectsAsync()
        {
            try
            {
                var subjects = await _service.GetAllSubjectsAsync();
                RefreshSubjects(subjects);
            }
            catch (Exception e)
            {
                OnMessageApplication(e.Message);
            }
        }

        private void RefreshSubjects(IEnumerable<SubjectDTO> subjectDTOs)
        {
            Subjects.Clear();

            foreach (var subjectDTO in subjectDTOs)
            {
                Subjects.Add(_mapper.Map<SubjectViewModel>(subjectDTO));
            }

            OnPropertyChanged(nameof(Subjects));
        }

        private void OnSubjectAddedEvent(SubjectViewModel s)
        {
            SubjectAddedEvent?.Invoke(this, s);
        }

        private void OnSubjectDeletedEvent(SubjectViewModel s)
        {
            SubjectDeletedEvent?.Invoke(this, s);
        }

        private void OnSubjectModifiedEvent(SubjectViewModel s)
        {
            SubjectModifiedEvent?.Invoke(this, s);
        }
    }
}