using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stretch4U.Business.ExercisesImages.Services;

namespace Stretch4U.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ExercisesController : ControllerBase
{
    private readonly IExercisesService _exercisesService;

    public ExercisesController(IExercisesService exercisesImageService)
    {
        _exercisesService = exercisesImageService;
    }

    [HttpPost("start-mobility-test/{userId}")]
    public async Task<IActionResult> StartMobilityTest([FromRoute] int userId, [FromForm(Name = "Data")] IFormFile image)
    {
        try
        {
            var result = await _exercisesService.StartMobilityTest(userId, image);
            return Ok(result);
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(e.Message);
        }
    }
}
