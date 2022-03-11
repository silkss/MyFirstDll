using System.Threading.Tasks;

namespace DataLayer.Interfaces;

public interface IRepository<T>
{
    Task<bool> CreateAsync(T entity);
    Task UpdateAsync(T source);
}
