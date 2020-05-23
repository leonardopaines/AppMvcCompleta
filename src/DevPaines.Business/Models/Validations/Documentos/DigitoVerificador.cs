using System.Collections.Generic;

namespace DevPaines.Business.Models.Validations.Documentos
{
    public class DigitoVerificador
    {
        private string _numero;
        private const int Modulo = 11;
        private readonly List<int> _multiplicadores = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly IDictionary<int, string> _substituicoes = new Dictionary<int, string>();
        private bool _complementarDoModulo = true;

        public DigitoVerificador(string numero)
        {
            this._numero = numero;
        }

        public DigitoVerificador ComMultiplicadoresDeAte(int primeiroMultiplicador, int ultimoMultiplicador)
        {
            this._multiplicadores.Clear();
            for (var i = primeiroMultiplicador; i <= ultimoMultiplicador; i++)
                this._multiplicadores.Add(i);

            return this;
        }

        public DigitoVerificador Substituindo(string substituto, params int[] digitos)
        {
            foreach (var i in digitos)
            {
                this._substituicoes[i] = substituto;
            }
            return this;
        }

        public void AddDigito(string digito)
        {
            this._numero = string.Concat(this._numero, digito);
        }

        public string CalculaDigito()
        {
            return !(this._numero.Length > 0) ? "" : this.GetDigitSum();
        }

        private string GetDigitSum()
        {
            var soma = 0;
            for (int i = this._numero.Length - 1, m = 0; i >= 0; i--)
            {
                var produto = (int)char.GetNumericValue(this._numero[i]) * this._multiplicadores[m];
                soma += produto;

                if (++m >= this._multiplicadores.Count) m = 0;
            }

            var mod = (soma % Modulo);
            var resultado = this._complementarDoModulo ? Modulo - mod : mod;

            return this._substituicoes.ContainsKey(resultado) ? this._substituicoes[resultado] : resultado.ToString();
        }
    }
}