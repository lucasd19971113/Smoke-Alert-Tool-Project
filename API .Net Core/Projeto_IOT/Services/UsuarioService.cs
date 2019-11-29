using Projeto_IOT.Models;
using Projeto_IOT.Repository.IRepository;
using Projeto_IOT.Services.IServices;

namespace Projeto_IOT.Services
{
    public class UsuarioService : BaseService<Usuario, IUsuarioRepository>, IUsuarioService
    {
        public UsuarioService(IUsuarioRepository repository) : base(repository)
        {
        }
    }
}