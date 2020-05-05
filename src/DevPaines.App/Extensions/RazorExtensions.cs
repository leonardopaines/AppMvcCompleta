using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPaines.App.Extensions
{
    public static class RazorExtensions
    {
        public static string FormataDocumento(this RazorPage page, int tipoPessoa, string documento)
        {
            string formatacao = tipoPessoa == 1 ? @"000\.000\.000\-00" : @"00\.000\.000\/0000\-00";
            return Convert.ToUInt64(documento).ToString(formatacao);
        }


    }
}
