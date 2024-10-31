using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public UserController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    // Custom endpoint to get user data with enrollment details
    [HttpGet("{userId}/enrollment")]
    public async Task<IActionResult> GetUserWithEnrollment(int userId)
    {
        // Define URLs for User Service and Enrollment Service
        var userUrl = $"https://localhost:5001/api/users/{userId}";
        var enrollmentUrl = $"https://localhost:5003/api/enrollments/user/{userId}";

        // Fetch user details from User Service
        var userResponse = await _httpClient.GetAsync(userUrl);
        if (!userResponse.IsSuccessStatusCode)
        {
            return NotFound("User not found");
        }

        var user = JsonSerializer.Deserialize<User>(
            await userResponse.Content.ReadAsStringAsync()
        );

        // Fetch enrollment details from Enrollment Service
        var enrollmentResponse = await _httpClient.GetAsync(enrollmentUrl);
        if (!enrollmentResponse.IsSuccessStatusCode)
        {
            return NotFound("Enrollments not found");
        }

        var enrollments = JsonSerializer.Deserialize<List<Enrollment>>(
            await enrollmentResponse.Content.ReadAsStringAsync()
        );

        // Aggregate data
        var result = new
        {
            User = user,
            Enrollments = enrollments
        };

        return Ok(result);
    }
}

// Model classes for demonstration (use your actual models)
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public string CourseName { get; set; }
}
