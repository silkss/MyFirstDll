using System.Threading.Tasks;

namespace DataLayer.Interfaces;

public interface IRepository<T>
{
    Task<T?> CreateAsync(T entity);
    Task UpdateAsync(T source);
}
