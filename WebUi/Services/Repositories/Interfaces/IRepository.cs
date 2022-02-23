using System.Threading.Tasks;

namespace WebUi.Services.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task CreateAsync(T item);

}
