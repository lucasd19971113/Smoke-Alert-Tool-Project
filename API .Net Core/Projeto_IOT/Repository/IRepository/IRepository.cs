using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projeto_IOT.Repository.IRepository
{
    public interface IRepository<T>
    {
        Task Add(T entity);
        Task Save();
        Task<T> GetById(int id);
        IEnumerable<T> GetAll();
        void Delete(T entity);
        void Update(T entity);
    }
}