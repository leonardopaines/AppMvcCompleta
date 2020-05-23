using AppMvcBasica.Models;
using DevPaines.Business.Interfaces;
using DevPaines.Business.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace DevPaines.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        protected BaseService(INotificador notificador) => this._notificador = notificador;

        protected void Notificar(string mensagem)
        {
            this._notificador.Handle(new Notificacao(mensagem));
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                this.Notificar(error.ErrorMessage);
            }
        }

        protected bool ExecutarValidacao<TV, TE>(TV validation, TE entity)
            where TV : AbstractValidator<TE>
            where TE : Entity
        {
            var validator = validation.Validate(entity);

            this.Notificar(validator);
            return validator.IsValid;
        }
    }
}