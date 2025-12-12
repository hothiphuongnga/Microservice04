using System.Net.Http.Json;
using System.Text.Json;
using Webapp.Models;

namespace Webapp.Services;

public interface IAuthService
{
    Task<ApiResponse<string>> LoginAsync(LoginRequest request);
    Task<ApiResponse<string>> RegisterAsync(RegisterRequest request);
}

public class AuthService : IAuthService
{    private readonly HttpClient client;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        // https://localhost:xxxx/user-service/api 
        client = httpClientFactory.CreateClient("UserApi");
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ApiResponse<string>> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await client.PostAsJsonAsync("Users/login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(_jsonOptions);
                return result ?? new ApiResponse<string> 
                { 
                    Success = false, 
                    Message = "Không thể đọc dữ liệu từ server" 
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return new ApiResponse<string>
            {
                Success = false,
                Message = $"Đăng nhập thất bại: {errorContent}",
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = $"Lỗi kết nối: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<string>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await client.PostAsJsonAsync("Users", request);
            Console.WriteLine(response.Content.ReadAsStringAsync());
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(_jsonOptions);
                return result ?? new ApiResponse<string> 
                { 
                    Success = true, 
                    Message = "Đăng ký thành công" 
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return new ApiResponse<string>
            {
                Success = false,
                Message = $"Đăng ký thất bại: {errorContent}",
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = $"Lỗi kết nối: {ex.Message}"
            };
        }
    }
}
