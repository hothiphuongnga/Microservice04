// unitofwork
namespace OrderService.Repositories.Base;
using OrderService.Data;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync();
}

public class UnitOfWork: IUnitOfWork
{

    private readonly OrderDbServiceContext _context;
    
    public UnitOfWork(OrderDbServiceContext context)
    {
        _context = context;
    }
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}


