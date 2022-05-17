using Microsoft.AspNetCore.WebUtilities;
using StudyMaterialShare.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace StudyMaterialShare.Desktop.Models
{
    public class StudyMaterialShareApiService : IStudyMaterialShareApiService
    {
        private readonly HttpClient _httpClient;

        public StudyMaterialShareApiService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7235")
            };
        }

        public async Task LoginAsync(LoginDTO user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Account/Admin/Login", user);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response,response.StatusCode);
            }

        }

        public async Task LogoutAsync()
        {
            HttpResponseMessage response = await _httpClient.PostAsync("api/Account/Logout", null);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }
        }

        public async Task<SubjectDTO> CreateSubjectAsync(SubjectDTO newSubject)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Subject", newSubject);

            if(!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<SubjectDTO>();
        }

        public async Task<IEnumerable<StudyMaterialDTO>> GetAllStudyMaterialsAsync()
        {
            var response = await _httpClient.GetAsync("api/StudyMaterial");

            if(!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<IEnumerable<StudyMaterialDTO>>();
        }

        public async Task<IEnumerable<StudyMaterialDTO>> GetAllStudyMaterialsAsync(int subjectId)
        {
            var response = await _httpClient.GetAsync(
                QueryHelpers.AddQueryString("api/StudyMaterial", "subjectId", subjectId.ToString()));

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<IEnumerable<StudyMaterialDTO>>();
        }

        public async Task<IEnumerable<StudyMaterialDTO>> GetAllStudyMaterialsAsync(string title)
        {
            var response = await _httpClient.GetAsync(
                QueryHelpers.AddQueryString("api/StudyMaterial", "title", title));

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<IEnumerable<StudyMaterialDTO>>();
        }

        public async Task<IEnumerable<StudyMaterialDTO>> GetAllStudyMaterialsAsync(string title, int subjectId)
        {
            var response = await _httpClient.GetAsync(
                QueryHelpers.AddQueryString("api/StudyMaterial",new Dictionary<string, string>()
                {
                    {"title", title},
                    {"subjectId", subjectId.ToString()}
                }));

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<IEnumerable<StudyMaterialDTO>>();
        }

        public async Task<IEnumerable<SubjectDTO>> GetAllSubjectsAsync()
        {
            var response = await _httpClient.GetAsync("api/Subject");

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<IEnumerable<SubjectDTO>>();
        }

        public async Task<StudyMaterialDTO> GetStudyMaterialByIdAsync(int studyMaterialId)
        {
            string requestUri = string.Format("api/StudyMaterial/{0}",studyMaterialId);
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<StudyMaterialDTO>();
        }

        public async Task<SubjectDTO> GetSubjectByIdAsync(int subjectId)
        {
            string requestUri = string.Format("api/Subject/{0}", subjectId);
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<SubjectDTO>();
        }

        public async Task<StudyMaterialDTO> RemoveRatingsFromStudyMaterialAsync(int studyMaterialId)
        {
            string requestUri = string.Format("api/StudyMaterial/{0}/ratings", studyMaterialId);
            var response = await _httpClient.DeleteAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<StudyMaterialDTO>();
        }

        public async Task<StudyMaterialDTO> RemoveStudyMaterialAsync(int studyMaterialId)
        {
            string requestUri = string.Format("api/StudyMaterial/{0}", studyMaterialId);
            var response = await _httpClient.DeleteAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<StudyMaterialDTO>();
        }

        public async Task<SubjectDTO> RemoveSubjectAsync(int subjectId)
        {
            string requestUri = string.Format("api/Subject/{0}", subjectId);
            var response = await _httpClient.DeleteAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<SubjectDTO>();
            }

            throw new NetworkException("Service returned response: " + response, response.StatusCode);
        }

        public async Task<StudyMaterialDTO> ResetStudyMaterialDownloadsAsync(StudyMaterialDTO studyMaterial)
        {
            studyMaterial.Downloads = 0;
            return await UpdateStudyMaterialAsync(studyMaterial);
        }

        public async Task<StudyMaterialDTO> UpdateStudyMaterialAsync(StudyMaterialDTO studyMaterial)
        {
            string requestUri = string.Format("api/StudyMaterial/{0}",studyMaterial.Id);
            var response = await _httpClient.PutAsJsonAsync(requestUri,studyMaterial);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<StudyMaterialDTO>();
        }

        public async Task<SubjectDTO> UpdateSubjectAsync(SubjectDTO subject)
        {
            string requestUri = string.Format("api/Subject/{0}", subject.Id);
            var response = await _httpClient.PutAsJsonAsync(requestUri, subject);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response, response.StatusCode);
            }

            return await response.Content.ReadAsAsync<SubjectDTO>();
        }
    }
}
