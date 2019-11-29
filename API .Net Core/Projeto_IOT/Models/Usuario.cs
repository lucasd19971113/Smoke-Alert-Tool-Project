using System;

namespace Projeto_IOT.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime Adicionado { get; set; } = DateTime.Now;
    }
}