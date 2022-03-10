using System.Threading.Tasks;

namespace DataLayer.Interfaces;

public interface IRepository<T>
{
    public Task<bool> CreateAsync(T entity);
}
