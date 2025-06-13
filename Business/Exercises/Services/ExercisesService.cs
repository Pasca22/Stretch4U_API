using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stretch4U.Business.Exercises.Models;
using Stretch4U.Data;
using Stretch4U.Data.Models.ExercisesImages;
using System.Net.Http.Json;
using System.Text.Json;

namespace Stretch4U.Business.ExercisesImages.Services;

internal class ExercisesService : IExercisesService
{
    private readonly Stretch4UContext _context;
    private readonly IConfiguration _configuration;
    private static HttpClient _httpClient;

    public ExercisesService(Stretch4UContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task<ExercisesData> StartMobilityTest(int userId, IFormFile image)
    {
        var url = _configuration[ConfigurationConstants.AIModelUrl];

        if (image == null || image.Length == 0)
        {
            throw new ArgumentNullException("Image not selected");
        }

        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);

        var hamstringsStretchImage = new HamstringStretchImage
        {
            FileName = image.FileName,
            Data = memoryStream.ToArray(),
            CreatedAt = DateTime.UtcNow,
            UserId = userId,
        };

        var imageInDB = _context.HamstringStretchImages.FirstOrDefault(i => i.UserId == userId);
        if (imageInDB == null)
        {
            await _context.HamstringStretchImages.AddAsync(hamstringsStretchImage);
        }
        else
        {
            imageInDB.FileName = image.FileName;
            imageInDB.Data = memoryStream.ToArray();
            imageInDB.CreatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        using var content = new MultipartFormDataContent();
        var imageContent = new ByteArrayContent(memoryStream.ToArray());
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
        content.Add(imageContent, "image", image.FileName);

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to communicate with AI model service: {response.ReasonPhrase}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var aiModelResponse = JsonSerializer.Deserialize<AIModelResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        user.MobilityLevel = aiModelResponse.Result;
        await _context.SaveChangesAsync();

        var exercises = await _context.Exercises.Where(e => e.MobilityLevel == aiModelResponse.Result)
                                                .ToListAsync();

        var result = new ExercisesData
        {
            Result = aiModelResponse.Result,
            Exercises = exercises
        };

        return result;
    }
}
