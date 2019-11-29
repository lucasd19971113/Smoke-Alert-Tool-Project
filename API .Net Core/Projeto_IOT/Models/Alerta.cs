using System;

namespace Projeto_IOT.Models
{
    public class Alerta
    {
        public int AlertaId { get; set; }
        public decimal RegistroGas { get; set; }
        public DateTime DataOcorrido { get; set; } = DateTime.Now;
    }
}