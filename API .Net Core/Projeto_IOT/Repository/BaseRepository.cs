using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projeto_IOT.Context;
using Projeto_IOT.Repository.IRepository;

namespace Projeto_IOT.Repository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext dbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task Add(T entity) => dbContext.Set<T>().AddAsync(entity);

        public virtual Task<T> GetById(int id) => dbContext.Set<T>().FindAsync(id);

        public virtual IEnumerable<T> GetAll() => dbContext.Set<T>().AsEnumerable();

        public void Delete(T entity) => dbContext.Set<T>().Remove(entity);

        public Task Save() => dbContext.SaveChangesAsync();
        
        public virtual void Update(T entity) => dbContext.Entry(entity).State = EntityState.Modified;
    }
}