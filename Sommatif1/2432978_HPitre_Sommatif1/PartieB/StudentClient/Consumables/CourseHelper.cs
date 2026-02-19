using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StudentClient.Configs;
using StudentClient.Models;

namespace StudentClient.Consumables
{
    public static class CourseHelper
    {
        private static readonly HttpClient client = new()
        {
            BaseAddress = new Uri(ConfigHelper.localAPIBaseUrl)
        };

        #region GET all/StudentsByCourse Methods
        public static async Task<List<Course>?> GetAllAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("Course");
                if (!response.IsSuccessStatusCode) return null;//If response = 200, return true, else false

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Course>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<Student>?> GetStudentsByCourseAsync(int courseId)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"Course/StudentsByCourse/{courseId}");
                if (!response.IsSuccessStatusCode)
                    return null;

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Student>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region POST Methods

        public static async Task<bool> PostRegisterStdCourseAsync(int CourseID, int StudentID)
        {
            HttpResponseMessage response = await client.PostAsync($"Course/registerStudentCourse/{CourseID}/{StudentID}",null);
            return response.IsSuccessStatusCode; //If response = 200, return true, else false
        }

        #endregion
    }
}
