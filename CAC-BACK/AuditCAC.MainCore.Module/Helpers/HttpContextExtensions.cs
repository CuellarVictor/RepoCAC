using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationValues<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double NoRegistrosTotales = await queryable.CountAsync();
            //double PaginasTotales = (NoRegistrosTotales * NoRegistrosPagina);

            //Agregamos Datos al Header del Request (API).
            httpContext.Response.Headers.Add("NoRegistrosTotales", NoRegistrosTotales.ToString());
        }
    }
}
