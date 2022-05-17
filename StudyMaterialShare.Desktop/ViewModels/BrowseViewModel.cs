using StudyMaterialShare.Desktop.ViewModel;
using StudyMaterialShare.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StudyMaterialShare.Desktop.Models;
using AutoMapper;
using System.Linq;

namespace StudyMaterialShare.Desktop.ViewModels
{
    public class BrowseViewModel : ViewModelBase
    {
        private readonly IStudyMaterialShareApiService _service;
        private readonly IMapper _mapper;
        private bool _isStudyMaterialModalOpen = false;
        private string _titleFilter = string.Empty;
        private SubjectViewModel _selectedSubject = null!;
        private StudyMaterialViewModel? _selectedStudyMaterial;
        private StudyMaterialViewModel? _editableStudyMaterial;
        private bool _isEnsureDeleteModalOpen;
        private bool _isResetDownloads = false;
        private bool _removeRatings = false;

        public bool IsStudyMaterialModalOpen
        {
            get => _isStudyMaterialModalOpen;
            set { _isStudyMaterialModalOpen = value; OnPropertyChanged(); }
        }

        public bool IsEnsureDeleteModalOpen
        {
            get => _isEnsureDeleteModalOpen;
            set { _isEnsureDeleteModalOpen = value; OnPropertyChanged(); }
        }

        public string TitleFilter
        {
            get => _titleFilter;
            set { _titleFilter = value; OnPropertyChanged(); }
        }

        public SubjectViewModel SelectedSubject
        {
            get => _selectedSubject;
            set { _selectedSubject = value; OnPropertyChanged(); }
        }

        public StudyMaterialViewModel? SelectedStudyMaterial
        {
            get => _selectedStudyMaterial;
            set { _selectedStudyMaterial = value; OnPropertyChanged(); }
        }

        public StudyMaterialViewModel? EditableStudyMaterial
        {
            get => _editableStudyMaterial;
            set { _editableStudyMaterial = value; OnPropertyChanged(); }
        }

        public bool IsResetDownloads
        {
            get => _isResetDownloads;
            set { _isResetDownloads = value; OnPropertyChanged(); }
        }

        public ObservableCollection<StudyMaterialViewModel> StudyMaterials { get; set; } = null!;

        public ObservableCollection<SubjectViewModel> Subjects { get; set; } = null!;

        public ICommand LoadStudyMaterialsCommand { get; set; }

        public ICommand StudyMaterialModalOpenCommand { get; set; }

        public ICommand StudyMaterialModalCloseCommand { get; set; }

        public ICommand StudyMaterialModalSaveCommand { get; set; }

        public ICommand StudyMaterialDeleteCommand { get; set; }

        public ICommand StudyMaterialBrowseCommand { get; set; }

        public ICommand EnsureDeleteModelOpenCommand { get; set; }

        public ICommand EnsureDeleteModalCloseCommand { get; set; }

        public bool RemoveRatings { get => _removeRatings; set => _removeRatings = value; }

        public BrowseViewModel(IStudyMaterialShareApiService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
            Subjects = new ObservableCollection<SubjectViewModel>();
            StudyMaterials = new ObservableCollection<StudyMaterialViewModel>();

            LoadStudyMaterialsCommand = new DelegateCommand(_ => LoadStudyMaterialAsync());

            StudyMaterialModalOpenCommand = new DelegateCommand(OnStudyMaterialModalOpen);
            StudyMaterialModalCloseCommand = new DelegateCommand(OnStudyMaterialModalClose);
            StudyMaterialModalSaveCommand = new DelegateCommand(OnStudyMaterialModalSave);

            StudyMaterialDeleteCommand = new DelegateCommand(OnStudyMaterialDelete);
            StudyMaterialBrowseCommand = new DelegateCommand(_ => OnStudyMaterialBrowse());

            EnsureDeleteModalCloseCommand = new DelegateCommand(OnEnsureDeleteModalClose);
            EnsureDeleteModelOpenCommand = new DelegateCommand(OnEnsureDeleteModalOpen);

            LoadSubjectsAsync();
            LoadStudyMaterialAsync();
        }

        private void OnEnsureDeleteModalOpen(object? obj)
        {
            SelectedStudyMaterial = obj as StudyMaterialViewModel;
            IsEnsureDeleteModalOpen = true;
        }

        private void OnEnsureDeleteModalClose(object? obj)
        {
            SelectedStudyMaterial = null;
            IsEnsureDeleteModalOpen = false;
        }

        private async void OnStudyMaterialDelete(object? obj)
        {
            try
            {
                IsEnsureDeleteModalOpen = false;

                var studyMaterial = obj as StudyMaterialViewModel;

                if (studyMaterial != null)
                    await _service.RemoveStudyMaterialAsync(studyMaterial.Id);

                LoadStudyMaterialAsync();
            }
            catch (Exception e)
            {
                OnMessageApplication(e.Message);
            }
        }

        private async void OnStudyMaterialModalSave(object? obj)
        {
            try
            {
                if (EditableStudyMaterial == null) return;

                //SelectedStudyMaterial?.CopyFrom(EditableStudyMaterial);

                _mapper.Map(EditableStudyMaterial, SelectedStudyMaterial);

                if (IsResetDownloads)
                {
                    EditableStudyMaterial.Downloads = 0;
                }

                if (RemoveRatings)
                {
                    await _service.RemoveRatingsFromStudyMaterialAsync(EditableStudyMaterial.Id);
                }
                await _service.UpdateStudyMaterialAsync(_mapper.Map<StudyMaterialDTO>(EditableStudyMaterial));

                SelectedStudyMaterial = null;
                EditableStudyMaterial = null;
                IsStudyMaterialModalOpen = false;
                IsResetDownloads = false;

                LoadStudyMaterialAsync();
            }
            catch (Exception e)
            {
                OnMessageApplication(e.Message);
            }
        }

        private void OnStudyMaterialModalClose(object? obj)
        {
            SelectedStudyMaterial = null;
            EditableStudyMaterial = null;
            IsStudyMaterialModalOpen = false;
        }

        private void OnStudyMaterialModalOpen(object? obj)
        {
            SelectedStudyMaterial = obj as StudyMaterialViewModel;
            EditableStudyMaterial = SelectedStudyMaterial?.ShallowClone();
            IsStudyMaterialModalOpen = true;
        }

        private async void OnStudyMaterialBrowse()
        {
            try
            {
                IEnumerable<StudyMaterialDTO> studyMaterials = null!;

                if (!string.IsNullOrEmpty(TitleFilter) && !string.IsNullOrEmpty(SelectedSubject?.Name))
                {
                    studyMaterials = await _service.GetAllStudyMaterialsAsync(TitleFilter, SelectedSubject.Id);
                }
                else if (!string.IsNullOrEmpty(TitleFilter))
                {
                    studyMaterials = await _service.GetAllStudyMaterialsAsync(TitleFilter);
                }
                else if (!string.IsNullOrEmpty(SelectedSubject?.Name))
                {
                    studyMaterials = await _service.GetAllStudyMaterialsAsync(SelectedSubject.Id);
                }
                else
                {
                    studyMaterials = await _service.GetAllStudyMaterialsAsync();
                }

                RefreshStudyMaterials(studyMaterials);
            }
            catch (Exception e)
            {
                OnMessageApplication(e.Message);
            }
        }

        private async void LoadStudyMaterialAsync()
        { 
            try
            {
                var studyMaterials = await _service.GetAllStudyMaterialsAsync();
                RefreshStudyMaterials(studyMaterials);
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

            Subjects = new ObservableCollection<SubjectViewModel>()
            {
                new SubjectViewModel()
                {
                    Name = String.Empty,
                }
            };

            foreach (var subjectDTO in subjectDTOs)
            {
                Subjects.Add(_mapper.Map<SubjectViewModel>(subjectDTO));
            }

            OnPropertyChanged(nameof(Subjects));
        }

        private void RefreshStudyMaterials(IEnumerable<StudyMaterialDTO> studyMaterialDTOs)
        {
            StudyMaterials.Clear();
            foreach (var studyMaterialDTO in studyMaterialDTOs)
            {
                StudyMaterials.Add(_mapper.Map<StudyMaterialViewModel>(studyMaterialDTO));
            }
            OnPropertyChanged(nameof(StudyMaterials));
        }
    }
}
