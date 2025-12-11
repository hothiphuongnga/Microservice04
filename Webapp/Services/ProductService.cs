using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using Webapp.Models;

namespace Webapp.Services;

public interface IProductService
{
    Task<ApiResponse<List<ProductDto>>> GetAllAsync();
    Task<ApiResponse<ProductDto>> GetByIdAsync(int id);
    Task<ApiResponse<ProductDto>> CreateAsync(CreateProductRequest request);
    Task<ApiResponse<ProductDto>> UpdateAsync(int id, ProductDto product);
    Task<ApiResponse<string>> DeleteAsync(int id);
}

public class ProductService : IProductService
{
    private readonly HttpClient _client;
    private readonly IJSRuntime _jsRuntime;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProductService(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime)
    {
        _client = httpClientFactory.CreateClient("ProductApi");
        _jsRuntime = jsRuntime;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private async Task<string?> GetTokenAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
        }
        catch
        {
            return null;
        }
    }

    public async Task<ApiResponse<List<ProductDto>>> GetAllAsync()
    {
        try
        {
            var response = await _client.GetAsync("Product");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductDto>>>(_jsonOptions);
                return result ?? new ApiResponse<List<ProductDto>>
                {
                    Success = false,
                    Message = "Không thể đọc dữ liệu từ server"
                };
            }

            return new ApiResponse<List<ProductDto>>
            {
                Success = false,
                Message = "Không thể lấy danh sách sản phẩm",
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<ProductDto>>
            {
                Success = false,
                Message = $"Lỗi kết nối: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync($"Product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>(_jsonOptions);
                return result ?? new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Không thể đọc dữ liệu từ server"
                };
            }

            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = "Không tìm thấy sản phẩm",
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = $"Lỗi kết nối: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<ProductDto>> CreateAsync(CreateProductRequest request)
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Vui lòng đăng nhập để thêm sản phẩm",
                    StatusCode = 401
                };
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsJsonAsync("Product", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>(_jsonOptions);
                return result ?? new ApiResponse<ProductDto>
                {
                    Success = true,
                    Message = "Thêm sản phẩm thành công"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = response.StatusCode == System.Net.HttpStatusCode.Unauthorized 
                    ? "Bạn chưa đăng nhập hoặc token không hợp lệ" 
                    : $"Thêm sản phẩm thất bại: {errorContent}",
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = $"Lỗi kết nối: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<ProductDto>> UpdateAsync(int id, ProductDto product)
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Vui lòng đăng nhập để cập nhật sản phẩm",
                    StatusCode = 401
                };
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PutAsJsonAsync($"Product/{id}", product);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>(_jsonOptions);
                return result ?? new ApiResponse<ProductDto>
                {
                    Success = true,
                    Message = "Cập nhật sản phẩm thành công"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = response.StatusCode == System.Net.HttpStatusCode.Unauthorized 
                    ? "Bạn chưa đăng nhập hoặc token không hợp lệ" 
                    : $"Cập nhật sản phẩm thất bại: {errorContent}",
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = $"Lỗi kết nối: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Vui lòng đăng nhập để xóa sản phẩm",
                    StatusCode = 401
                };
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.DeleteAsync($"Product/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Xóa sản phẩm thành công"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return new ApiResponse<string>
            {
                Success = false,
                Message = response.StatusCode == System.Net.HttpStatusCode.Unauthorized 
                    ? "Bạn chưa đăng nhập hoặc token không hợp lệ" 
                    : $"Xóa sản phẩm thất bại: {errorContent}",
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
