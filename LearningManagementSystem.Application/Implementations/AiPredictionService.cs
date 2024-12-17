using LearningManagementSystem.Application.Dtos.Ai;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Implementations
{
    public class AiPredictionService : IAiPredictionService
    {
        private readonly HttpClient _httpClient;

        public AiPredictionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PredictionResponse> PredictCourse(UserData userData)
        {
            try
            {
                var requestBody = new
                {
                    age = userData.Age,
                    studytime = userData.StudyTime,
                    absences = userData.Absences,
                    failures = userData.Failures
                };

                string jsonPayload = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("http://localhost:5000/predict", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API Error: {errorContent}");
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var predictionResult = JsonConvert.DeserializeObject<PredictionResponse>(responseContent);

                return predictionResult;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error while connecting to AI Prediction API.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Error while parsing the AI Prediction API response.", ex);
            }
        }
    }
}
