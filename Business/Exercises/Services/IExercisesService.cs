using Microsoft.AspNetCore.Http;
using Stretch4U.Business.Exercises.Models;

namespace Stretch4U.Business.ExercisesImages.Services;

public interface IExercisesService
{
    Task<ExercisesData> StartMobilityTest(int userId, IFormFile image);
}
