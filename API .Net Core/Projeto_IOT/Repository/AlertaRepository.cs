using Projeto_IOT.Context;
using Projeto_IOT.Models;
using Projeto_IOT.Repository.IRepository;

namespace Projeto_IOT.Repository
{
    public class AlertaRepository : BaseRepository<Alerta>, IAlertaRepository
    {
    
        public AlertaRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}