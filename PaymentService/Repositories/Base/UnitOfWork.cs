// unitofwork
namespace PaymentService.Repositories.Base;
using PaymentService.Data;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync();
}

public class UnitOfWork: IUnitOfWork
{

    private readonly PaymentDbServiceContext _context;
    
    public UnitOfWork(PaymentDbServiceContext context)
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


