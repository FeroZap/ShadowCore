using System.Threading;
using System.Threading.Tasks;
using ShadowBox.AutomaticDI.Interfaces;

namespace DotnetCoreAngularStarter.DAL.Abstract
{
    public interface IUnitOfWork : IScopedLifetime
    {
        IRepository<T> Repository<T>() where T : class;
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
    }
}
