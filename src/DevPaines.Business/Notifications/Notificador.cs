using DevPaines.Business.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DevPaines.Business.Notifications
{
    public class Notificador : INotificador
    {
        private List<Notificacao> _notificacoes;

        public Notificador()
        {
            this._notificacoes = new List<Notificacao>();
        }

        public void Handle(Notificacao notificacao) => this._notificacoes.Add(notificacao);

        public List<Notificacao> ObterNotificacoes() => this._notificacoes;

        public bool TemNotificacao() => this._notificacoes.Any();
    }
}
