using DevPaines.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevPaines.App.Controllers
{
    public abstract class BaseController : Controller
    {
        public readonly INotificador _notificador;

        protected BaseController(INotificador notificado) => this._notificador = notificado;

        public bool OperacaoValida() => !this._notificador.TemNotificacao();
    }
}