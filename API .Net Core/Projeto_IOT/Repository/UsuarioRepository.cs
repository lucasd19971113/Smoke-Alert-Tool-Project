using Projeto_IOT.Context;
using Projeto_IOT.Models;
using Projeto_IOT.Repository.IRepository;
using Projeto_IOT.Services.IServices;

namespace Projeto_IOT.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}