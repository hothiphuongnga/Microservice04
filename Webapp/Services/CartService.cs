using Webapp.Models;

using Microsoft.JSInterop;
using System.Text.Json;
public interface ICartService
{
    // cart lưu local 
    //get cart
    Task<List<CartItem>> GetCartItemAsync();
    //them gio hang
    Task AddCartAsync(ProductDto item, int quantity);
    //xoa gio hang
    Task RemoveCartItemAsync(int productId);
    //cap nhat so luong
    Task UpdateCartItemAsync(int productId, int quantity);
    // clear cart
    //lấy số lượng sản phẩm trong giỏ hàng
    Task<int> CountCartItemAsync();
    Task CleanCartAsync();

}

public class CartService : ICartService
{
    private readonly IJSRuntime _jsRuntime;
    private const string CartKey = "cart"; // biến lưu key trong local storage

    public CartService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<List<CartItem>> GetCartItemAsync()
    {
        try
        {
            var cartString = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", CartKey);
            if (string.IsNullOrEmpty(cartString)) // nếu không có cart thì khởi tạo list rỗng
            {
                return new List<CartItem>();
            }
            return JsonSerializer.Deserialize<List<CartItem>>(cartString) ?? new List<CartItem>();
        }
        catch (Exception ex)
        {
            return new List<CartItem>();
        }
    }

    public async Task AddCartAsync(ProductDto item, int quantity)
    {
        // kiểm tra sản phẩm đã có trong giỏ hàng chưa
        var lsCart = await GetCartItemAsync();
        var cartItem = lsCart.Find(ci => ci.ProductId == item.Id);
        if (cartItem == null)
        {
            // thêm mới
            cartItem = new CartItem
            {
                ProductId = item.Id,
                Name = item.Name,
                Price = item.Price,
                Quantity = quantity,
                Stock = item.Stock
            };
            lsCart.Add(cartItem);
        }
        else
        {
            // cập nhật số lượng
            cartItem.Quantity += quantity;
        }
        // lưu lại giỏ hàng vào local storage
        await SaveCartAsync(lsCart);
    }

    public async Task RemoveCartItemAsync(int productId)
    {
        var lsCart = await GetCartItemAsync();
        // kiểm tra 
        var find = lsCart.Find(a => a.ProductId == productId);
        if (find != null)
        {
            lsCart.Remove(find);
        }
        await SaveCartAsync(lsCart);

    }

    public async Task UpdateCartItemAsync(int productId, int quantity)
    {
        // ktra tôn tại 
        var lsCart = await GetCartItemAsync();
        var find = lsCart.Find(a => a.ProductId == productId);

        // update số lượng
        if (find != null)
        {
            find.Quantity += quantity;
        }
        await SaveCartAsync(lsCart);

    }

    public async Task CleanCartAsync()
    {
        await SaveCartAsync(new List<CartItem>());
    }

    private async Task SaveCartAsync(List<CartItem> cart)
    {
        string cartString = JsonSerializer.Serialize(cart);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", CartKey, cartString);
    }

    public async Task<int> CountCartItemAsync()
    {
        var lsCart = await GetCartItemAsync();

        // 4 sp mỗi sp lần lượt là 3 4 5 2 món 
        return lsCart.Sum(a => a.Quantity);  // tổng của tất cả các món
    }
}