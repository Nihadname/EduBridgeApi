using LearningManagementSystem.Application.Dtos.Ai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IAiPredictionService
    {
        Task<PredictionResponse> PredictCourse(UserData userData);
    }
}
