using LearningManagementSystem.Application.Dtos.Ai;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Implementations
{
    public class AiPredictionService: IAiPredictionService
    {
        private readonly HttpClient _httpClient;

        public AiPredictionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PredictionResponse> PredictCourse(UserData userData)
        {
            var url = "http://localhost:5000/predict";
            try
            {
                var jsonPayload = JsonConvert.SerializeObject(new
                {
                    age = userData.Age,
                    isParent = userData.IsParent,
                    childAge = userData.ChildAge 
                },
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include 
        });
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var prediction = JsonConvert.DeserializeObject<PredictionResponse>(result);

                    return new PredictionResponse
                    {
                        predicted_course = prediction.predicted_course,
                    };
                }
                else
                {
                    throw new CustomException(500, "problem occured");
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(500,ex.Message);

            }
        }
    }
}
