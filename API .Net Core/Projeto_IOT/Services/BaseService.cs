using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Projeto_IOT.Repository.IRepository;
using Projeto_IOT.Services.IServices;
using Projeto_IOT.Shared;

namespace Projeto_IOT.Services
{
    public abstract class BaseService<T, Repository> : IService<T>
        where Repository : IRepository<T>
        where T : class
    {
        protected readonly Repository repository;


        public BaseService(Repository repository)
        {
            this.repository = repository;
            
        }

        public async Task<Result> Add(T entity)
        {
            await repository.Add(entity);
            await repository.Save();
            return Result.Ok();
        }

        public virtual async Task<Result<T>> GetById(int id)
        {
            var entity = await repository.GetById(id);
            if (entity != null)
            {
                return Result.Ok(entity);
            }

            return Result.Fail<T>("Nenhum registro com o ID encontrado", ResultCode.NotFound);
        }

        public async Task<Result<List<T>>> GetAll()
        {
            var entities = await repository.GetAll()
                .AsQueryable()
                .ToListAsync();

            if (entities.Count > 0)
            {
                return Result.Ok(entities);
            }

            return Result.Fail<List<T>>("Nenhum registro encontrado nessa tabela", ResultCode.NotFound);
        }

        public async Task<Result> Delete(int id)
        {
            var entity = await repository.GetById(id);
            if (entity != null)
            {
                try
                {
                    repository.Delete(entity);
                    await repository.Save();
                    return Result.Ok();
                }
                catch (DbUpdateException)
                {
                    return Result.Fail("Não é possível deletar essa entidade pois outra depende da mesma", ResultCode.BadRequest);
                }
            }
             
            return Result.Fail("Nenhum registro com o ID encontrado", ResultCode.NotFound);
        }
    }
}