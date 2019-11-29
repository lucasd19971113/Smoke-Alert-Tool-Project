using System.Collections.Generic;
using System.Threading.Tasks;
using Projeto_IOT.Shared;

namespace Projeto_IOT.Services.IServices
{
    public interface IService<T>
    {
        Task<Result<T>> GetById(int id);
        Task<Result<List<T>>> GetAll();
        Task<Result> Add(T entity);
        Task<Result> Delete(int id);
    }
}