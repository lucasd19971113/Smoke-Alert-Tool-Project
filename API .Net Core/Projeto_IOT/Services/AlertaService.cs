using Projeto_IOT.Models;
using Projeto_IOT.Repository.IRepository;
using Projeto_IOT.Services.IServices;

namespace Projeto_IOT.Services
{
    public class AlertaService : BaseService<Alerta, IAlertaRepository>, IAlertaService
    {
        public AlertaService(IAlertaRepository repository) : base(repository)
        {
        }
    }
}