using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Actas;
using AuditCAC.Domain.Helpers;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class ActaManager : IActaManager<ParametroTemplateDto>
    {
        private readonly DBAuditCACContext context;
        private readonly ILogger<ParametroTemplateDto> _logger;

        public ActaManager(DBAuditCACContext _context, ILogger<ParametroTemplateDto> logger)
        {
            this.context = _context;
            _logger = logger;

            this.context.Database.SetCommandTimeout(5000);
        }


        /// <summary>
        /// Consulta parametros de los templates
        /// </summary>
        /// <returns>Lista Modelo parametros</returns>
        public async Task<List<ParametroTemplateDto>> ConsultaParametrosTemplate()
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consulta_Parametros_Template] ";

                List<SqlParameter> parms = new List<SqlParameter>
                {

                };

                var Data = await context.ParametroTemplateDto.FromSqlRaw<ParametroTemplateDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Crea o actualiza parametro de los templates
        /// </summary>
        /// <param name="input">Modelo parametro</param>
        /// <returns>true</returns>
        public async Task<bool> UpsertParametroTemplate(ParametroTemplateDto input)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Upsert_Parametro_Template] @Id, @Template, @TipoInput, @TipoDato, @Parametro, @Valor, @Descripcion, @User, @Obligatorio";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Id", Value = input.Id},
                    new SqlParameter { ParameterName = "@Template", Value = input.Template},
                    new SqlParameter { ParameterName = "@TipoInput", Value = input.TipoInput},
                    new SqlParameter { ParameterName = "@TipoDato", Value = input.TipoDato},
                    new SqlParameter { ParameterName = "@Parametro", Value = input.Parametro},
                    new SqlParameter { ParameterName = "@Valor", Value = input.Valor},
                    new SqlParameter { ParameterName = "@Descripcion", Value = input.Descripcion},
                    new SqlParameter { ParameterName = "@User", Value = input.ModifyBy },
                    new SqlParameter { ParameterName = "@Obligatorio", Value = input.Obligatorio }
                };

                var data = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<string> GenerarActaApertura(GenerarActaInputDto inputDto)
        {
            // Verificar si el template es el adecuado
            if (inputDto.IdTemplate != (int)Enumeration.Actas.Template.ActaApertura)
                return null;

            // Obtener plantilla para la generacion
            string htmlTemplate = await GetTemplateFileActaAperturaHtml();

            // Leer el total de parametros desde DB
            IEnumerable<ParametroTemplateDto> listParams = await ConsultaParametrosTemplate();

            // Indice de claves de parametros revisados
            List<string> keysReviewed = new();

            // Buscar todos los parametros en el listado segun input
            foreach (ParametroDto inputParam in inputDto.ListaParametros)
            {
                // Verificar si ya fue procesado (para usuario pueden
                // haber multiples entradas que no queremos repetir)
                if (keysReviewed.Contains(inputParam.ParametroTemplateKey))
                    continue;

                ParametroTemplateDto param = listParams.FirstOrDefault(item =>
                                                item.Template == inputDto.IdTemplate &&
                                                item.Parametro == inputParam.ParametroTemplateKey);

                if (param != null)
                {
                    // Parametro encontrado, se obtiene el valor
                    switch (param.TipoInput)
                    {
                        case (int)Enumeration.Actas.TipoInput.Parametrizacion:
                            htmlTemplate = htmlTemplate.Replace(param.Parametro, param.Valor);
                            break;
                        case (int)Enumeration.Actas.TipoInput.Usuario:
                            htmlTemplate = htmlTemplate.Replace(inputParam.ParametroTemplateKey, inputParam.Value);
                            break;
                        case (int)Enumeration.Actas.TipoInput.Consultado:
                            break;
                        default:
                            // Parametro desconocido
                            break;
                    }

                    keysReviewed.Add(inputParam.ParametroTemplateKey);
                }
                else
                {
                    // Parametro no encontrado en la lista de DB

                    string[] family = inputParam.ParametroTemplateKey.Split(".");
                    if (family.Length != 2)
                        continue;

                    string parentKey = family[0];
                    string childKey = family[1];

                    const string constListado = Enumeration.Actas.Listado;

                    if ((parentKey == constListado && childKey == Enumeration.Actas.AperturaParamsConstantes.TablaMedicionNombres) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.AperturaParamsConstantes.TablaMedicionCasos))
                    {
                        ConsultarActaMediciones(inputDto.IdCobertura, inputDto.IdEPS, ref htmlTemplate);
                        keysReviewed.Add(Enumeration.Actas.AperturaParamsConstantes.TablaMedicion);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.AperturaParamsConstantes.TablaMedicionNombres);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.AperturaParamsConstantes.TablaMedicionCasos);
                    }
                    else
                    if ((parentKey == constListado && childKey == Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesCACNombres) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesCACCargos))
                    {
                        GenerarColTablaActaApertura(inputDto.ListaParametros,
                                                        Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesCAC,
                                                        Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesCACNombres,
                                                        Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesCACCargos,
                                                        ref htmlTemplate);
                        keysReviewed.Add(Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesCAC);
                        keysReviewed.Add(Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesCACNombres);
                        keysReviewed.Add(Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesCACCargos);
                    }
                    else
                    if ((parentKey == constListado && childKey == Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesEntidadNombres) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesEntidadNombres))
                    {
                        GenerarColTablaActaApertura(inputDto.ListaParametros,
                                                        Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesEntidad,
                                                        Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesEntidadNombres,
                                                        Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesEntidadCargos,
                                                        ref htmlTemplate);
                        keysReviewed.Add(Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesEntidad);
                        keysReviewed.Add(Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesEntidadNombres);
                        keysReviewed.Add(Enumeration.Actas.AperturaParamsConstantes.TablaAsistentesEntidadNombres);
                    }
                }
            }

            IEnumerable<string> paramsToQuery =
            listParams.Where(item => item.Template == inputDto.IdTemplate &&
                                     item.TipoInput == (int)Enumeration.Actas.TipoInput.Consultado)
                      .Select(p => p.Parametro);

            var data = ConsultarParametrosActa(inputDto.IdCobertura, inputDto.IdEPS, paramsToQuery, ref htmlTemplate);

            //Captura fecha de corte
            var fechaCorte = data.Where(x => x.ParametroTemplateKey == "$FechaCorte$").Select(x => x.Value).FirstOrDefault();

            //Captura cobertura
            var cobertura = data.Where(x => x.ParametroTemplateKey == "$Cobertura$").Select(x => x.Value).FirstOrDefault();

            //Construir tabla auditores y funciones
            ConsultarAuditoresCoberturamedicion(inputDto.IdCobertura, inputDto.IdEPS, ref htmlTemplate);

            // Parse html to PDF
            var fileBytes = ConvertHTMLToPDF(htmlTemplate, "v", fechaCorte, cobertura, "APERTURA");
            string file = Convert.ToBase64String(fileBytes);

            return file;
        }


        public void GenerarColTablaActaApertura(IEnumerable<ParametroDto> listaParametros, string tabla,
                                                string keyNombres, string keyValores, ref string htmlTemplate)
        {
            // Determinacion de condiciones iniciales para elaboracion de la tabla
            string keyName = Enumeration.Actas.Listado + "." + keyNombres;
            string keyValue = Enumeration.Actas.Listado + "." + keyValores;

            IEnumerable<ParametroDto> listNombres = listaParametros.Where(item => item.ParametroTemplateKey == keyName).ToList();
            IEnumerable<ParametroDto> listValores = listaParametros.Where(item => item.ParametroTemplateKey == keyValue).ToList();
            string content = string.Empty;

            // Elaboracion de la tabla
            try
            {
                for (int index = 0; index < listNombres.Count(); index++)
                {
                    content +=
                    @$"<tr>
                    <td style=""text-align: center;border:1px solid;"">
                        {listNombres.ElementAt(index).Value}
                    </td>
                    <td style=""text-align: center;border:1px solid;"" colspan=""2"">
                        {listValores.ElementAt(index).Value}
                    </td>
                </tr>
                ";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            htmlTemplate = htmlTemplate.Replace(tabla, content);
        }

        public List<ParametroDto> ConsultarParametrosActa(int idCobertura, string idEPS, IEnumerable<string> paramsToQuery,
                                            ref string htmlTemplate)
        {
            string keyParams = string.Join(",", paramsToQuery);
            string sql = "EXEC [dbo].[SP_Consulta_ActasAuditoria_Apertura] @idCobertura, @idEPS, @keyParams";

            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@idCobertura", Value = idCobertura },
                    new SqlParameter { ParameterName = "@idEPS", Value = idEPS },
                    new SqlParameter { ParameterName = "@keyParams", Value = keyParams }
                };

            var data = context.ParametroDto.FromSqlRaw<ParametroDto>(sql, parms.ToArray()).ToList();

            foreach (ParametroDto parametroDto in data)
                htmlTemplate = htmlTemplate.Replace(parametroDto.ParametroTemplateKey, parametroDto.Value);

            return data;
        }

        public void ConsultarActaMediciones(int idCobertura, string idEPS, ref string htmlTemplate)
        {
            string sql = "EXEC [dbo].[SP_Consulta_Mediciones_Actas] @idCobertura, @idEPS";

            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@idCobertura", Value = idCobertura },
                    new SqlParameter { ParameterName = "@idEPS", Value = idEPS }
                };

            IEnumerable<ParametroMedicionDto> data = context.ParametroMedicionDto.FromSqlRaw(sql, parms.ToArray()).ToList();

            string content = string.Empty;

            // Elaboracion de la tabla de Mediciones
            try
            {
                foreach (ParametroMedicionDto parametroMedicionDto in data)
                {
                    content +=
                    @$"<tr>
                    <td style=""text-align: center;border:1px solid;"">
                        {parametroMedicionDto.Description}
                    </td>
                    <td style=""text-align: center;border:1px solid;"" colspan=""2"">
                        {parametroMedicionDto.Count}
                    </td>
                </tr>
                ";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            htmlTemplate = htmlTemplate.Replace(Enumeration.Actas.AperturaParamsConstantes.TablaMedicion, content);
        }



        public void ConsultarAuditoresCoberturamedicion(int idCobertura, string idEPS, ref string htmlTemplate)
        {
            string sql = "EXEC [dbo].[SP_Consulta_Auditores_EPS_Cobertura] @idCobertura, @idEPS";

            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@idCobertura", Value = idCobertura },
                    new SqlParameter { ParameterName = "@idEPS", Value = idEPS }
                };

            IEnumerable<AuditorCoberturaEpsDto> data = context.AuditorCoberturaEpsDto.FromSqlRaw(sql, parms.ToArray()).ToList();

            string content = string.Empty;

            // Elaboracion de la tabla de Mediciones
            try
            {
                foreach (AuditorCoberturaEpsDto item in data)
                {
                    content +=
                    @$"<tr>
                    <td style=""text-align: center;border:1px solid;"">
                        {item.Nombres}
                    </td>
                    <td style=""text-align: center;border:1px solid;"" colspan=""2"">
                        {item.Funcion}
                    </td>
                </tr>
                ";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            htmlTemplate = htmlTemplate.Replace(Enumeration.Actas.AperturaParamsConstantes.TablaGrupoAuditor, content);
        }



        public async Task<string> GenerarActaCierre(GenerarActaInputDto inputDto)
        {
            // Verificar si el template es el adecuado
            if (inputDto.IdTemplate != (int)Enumeration.Actas.Template.ActaCierre)
                return null;

            // Obtener plantilla para la generacion
            string htmlTemplate = await GetTemplateFileActaCierreHtml();

            // Leer el total de parametros desde DB
            IEnumerable<ParametroTemplateDto> listParams = await ConsultaParametrosTemplate();

            // Indice de claves de parametros revisados
            List<string> keysReviewed = new();

            // Buscar todos los parametros en el listado segun input
            foreach (ParametroDto inputParam in inputDto.ListaParametros)
            {
                // Verificar si ya fue procesado (para usuario pueden
                // haber multiples entradas que no queremos repetir)
                if (keysReviewed.Contains(inputParam.ParametroTemplateKey))
                    continue;

                ParametroTemplateDto param = listParams.FirstOrDefault(item =>
                                                item.Template == inputDto.IdTemplate &&
                                                item.Parametro == inputParam.ParametroTemplateKey);

                if (param != null)
                {
                    // Parametro encontrado, se obtiene el valor
                    switch (param.TipoInput)
                    {
                        case (int)Enumeration.Actas.TipoInput.Parametrizacion:
                            htmlTemplate = htmlTemplate.Replace(param.Parametro, param.Valor);
                            break;
                        case (int)Enumeration.Actas.TipoInput.Usuario:
                            htmlTemplate = htmlTemplate.Replace(inputParam.ParametroTemplateKey, inputParam.Value);
                            break;
                        case (int)Enumeration.Actas.TipoInput.Consultado:
                            break;
                        default:
                            // Parametro desconocido
                            break;
                    }

                    keysReviewed.Add(inputParam.ParametroTemplateKey);
                }
                else
                {
                    // Parametro no encontrado en la lista de DB

                    string[] family = inputParam.ParametroTemplateKey.Split(".");
                    if (family.Length != 2)
                        continue;

                    string parentKey = family[0];
                    string childKey = family[1];

                    const string constListado = Enumeration.Actas.Listado;

                    if ((parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadItem) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadSi) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadNo) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadObservacion))
                    {
                        GenerarColTablaDetalleActaCierre(inputDto.ListaParametros, ref htmlTemplate);
                        keysReviewed.Add(Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidad);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadItem);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadSi);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadNo);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadObservacion);
                    }
                    else
                    if ((parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadItem) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadConformes) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadNoConformes) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadNoDisponible) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadCasosAuditados))
                    {
                        ConsultarMedicionesDetalleActaCierre(inputDto.IdCobertura, inputDto.IdEPS, ref htmlTemplate);
                        keysReviewed.Add(Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidad);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadItem);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadConformes);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadNoConformes);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadNoDisponible);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidadCasosAuditados);
                    }
                    else
                    if ((parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistenciasIxDiagnostico) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistenciasIxSoportes) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistenciasCasosAuditados) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistenciasGlosax100))
                    {
                        ConsultarMedicionesInconsistenciasActaCierre(inputDto.IdCobertura, inputDto.IdEPS, ref htmlTemplate);
                        keysReviewed.Add(Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistencias);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistenciasIxDiagnostico);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistenciasIxSoportes);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistenciasCasosAuditados);
                        keysReviewed.Add(constListado + "." + Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistenciasGlosax100);
                    }
                    else
                    if ((parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaAsistentesCACNombres) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaAsistentesCACCargos))
                    {
                        GenerarColTablaActaApertura(inputDto.ListaParametros,
                                                        Enumeration.Actas.CierreParamsConstantes.TablaAsistentesCAC,
                                                        Enumeration.Actas.CierreParamsConstantes.TablaAsistentesCACNombres,
                                                        Enumeration.Actas.CierreParamsConstantes.TablaAsistentesCACCargos,
                                                        ref htmlTemplate);
                        keysReviewed.Add(Enumeration.Actas.CierreParamsConstantes.TablaAsistentesCAC);
                        keysReviewed.Add(Enumeration.Actas.CierreParamsConstantes.TablaAsistentesCACNombres);
                        keysReviewed.Add(Enumeration.Actas.CierreParamsConstantes.TablaAsistentesCACCargos);
                    }
                    else
                    if ((parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaAsistentesEntidadNombres) ||
                        (parentKey == constListado && childKey == Enumeration.Actas.CierreParamsConstantes.TablaAsistentesEntidadNombres))
                    {
                        GenerarColTablaActaApertura(inputDto.ListaParametros,
                                                        Enumeration.Actas.CierreParamsConstantes.TablaAsistentesEntidad,
                                                        Enumeration.Actas.CierreParamsConstantes.TablaAsistentesEntidadNombres,
                                                        Enumeration.Actas.CierreParamsConstantes.TablaAsistentesEntidadCargos,
                                                        ref htmlTemplate);
                        keysReviewed.Add(Enumeration.Actas.CierreParamsConstantes.TablaAsistentesEntidad);
                        keysReviewed.Add(Enumeration.Actas.CierreParamsConstantes.TablaAsistentesEntidadNombres);
                        keysReviewed.Add(Enumeration.Actas.CierreParamsConstantes.TablaAsistentesEntidadNombres);
                    }
                }
            }
            
            // Query params
            List<string> paramsToQuery =
            listParams.Where(item => item.Template == inputDto.IdTemplate &&
                                     item.TipoInput == (int)Enumeration.Actas.TipoInput.Consultado)
                      .Select(p => p.Parametro).ToList();

            var data = ConsultarParametrosActa(inputDto.IdCobertura, inputDto.IdEPS, paramsToQuery, ref htmlTemplate);

            //Captura fecha de corte
            var fechaCorte = data.Where(x => x.ParametroTemplateKey == "$FechaCorte$").Select(x => x.Value).FirstOrDefault();

            //Captura cobertura
            var cobertura = data.Where(x => x.ParametroTemplateKey == "$Cobertura$").Select(x => x.Value).FirstOrDefault();

            //Construir tabla auditores y funciones
            ConsultarAuditoresCoberturamedicion(inputDto.IdCobertura, inputDto.IdEPS, ref htmlTemplate);

            var fileBytes = ConvertHTMLToPDF(htmlTemplate, "v", fechaCorte, cobertura, "CIERRE");
            string file = Convert.ToBase64String(fileBytes);

            return file;


        }

        public void GenerarColTablaDetalleActaCierre(IEnumerable<ParametroDto> listaParametros, 
                                                     ref string htmlTemplate)
        {
            const string TablaDetallesEntidad = Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidad;
            const string TablaDetallesEntidadItem = Enumeration.Actas.Listado + "." +
                                                    Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadItem;
            const string TablaDetallesEntidadSi = Enumeration.Actas.Listado + "." +
                                                  Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadSi;
            const string TablaDetallesEntidadNo = Enumeration.Actas.Listado + "." +
                                                  Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadNo;
            const string TablaDetallesEntidadObservacion = Enumeration.Actas.Listado + "." +
                                                           Enumeration.Actas.CierreParamsConstantes.TablaDetallesEntidadObservacion;
            
            IEnumerable<ParametroDto> listItems = listaParametros.Where(item =>
                                                        item.ParametroTemplateKey == TablaDetallesEntidadItem).ToList();
            IEnumerable<ParametroDto> listOpcionSi = listaParametros.Where(item =>
                                                        item.ParametroTemplateKey == TablaDetallesEntidadSi).ToList();
            IEnumerable<ParametroDto> listOpcionNo = listaParametros.Where(item =>
                                                        item.ParametroTemplateKey == TablaDetallesEntidadNo).ToList();
            IEnumerable<ParametroDto> listObservacion = listaParametros.Where(item =>
                                                        item.ParametroTemplateKey == TablaDetallesEntidadObservacion).ToList();

            string content = string.Empty;

            // Elaboracion de la tabla
            try
            {
                for (int index = 0; index < listItems.Count(); index++)
                {
                    content +=
                    @$"<tr>
                    <td style=""border: 1px solid #000;"">
                        {listItems.ElementAt(index).Value}
                    </td>
                    <td style=""border: 1px solid #000;"">
                        {listOpcionSi.ElementAt(index).Value}
                    </td>
                    <td style=""border: 1px solid #000;"">
                        {listOpcionNo.ElementAt(index).Value}
                    </td>
                    <td style=""border: 1px solid #000;"">
                        {listObservacion.ElementAt(index).Value}
                    </td>
                </tr>
                ";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            htmlTemplate = htmlTemplate.Replace(TablaDetallesEntidad, content);
        }

        public void ConsultarMedicionesDetalleActaCierre(int idCobertura, string idEPS, ref string htmlTemplate)
        {
            string sql = "EXEC [dbo].[SP_Consulta_ActaCierre_MedicionesDetalle] @idCobertura, @idEPS";

            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@idCobertura", Value = idCobertura },
                    new SqlParameter { ParameterName = "@idEPS", Value = idEPS }
                };

            IEnumerable<ParametroMedicionDetalleDto> data = context.ParametroMedicionDetalleDto.FromSqlRaw(sql, parms.ToArray()).ToList();

            string content = string.Empty;

            // Elaboracion de la tabla de Mediciones
            try
            {
                foreach (ParametroMedicionDetalleDto parametroMedicionDto in data)
                {
                    content +=
                    @$"<tr>
					    <td style=""border: 1px solid #000;text-align: center"">
						    {parametroMedicionDto.Medicion}
					    </td>
					    <td style=""border: 1px solid #000;text-align: center"">
						    {parametroMedicionDto.Conforme:0.0}
					    </td>
					    <td style=""border: 1px solid #000;text-align: center"">
						    {parametroMedicionDto.NoConforme:0.0}
					    </td>
					    <td style=""border: 1px solid #000;text-align: center"">
						    {parametroMedicionDto.NoDisponible}
					    </td>
					    <td style=""border: 1px solid #000;text-align: center"">
						    {Convert.ToInt32(parametroMedicionDto.CasosAuditados)}
					    </td>
				    </tr>
                    ";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            htmlTemplate = htmlTemplate.Replace(Enumeration.Actas.CierreParamsConstantes.TablaMedicionCalidad, content);
        }

        public void ConsultarMedicionesInconsistenciasActaCierre(int idCobertura, string idEPS, 
                                                                 ref string htmlTemplate)
        {
            string sql = "EXEC [dbo].[SP_Consulta_ActaCierre_MedicionesInconsistencias] @idCobertura, @idEPS";

            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@idCobertura", Value = idCobertura },
                    new SqlParameter { ParameterName = "@idEPS", Value = idEPS }
                };

            IEnumerable<ParametroMedicionInconsistenciaDto> data = context.ParametroMedicionInconsistenciaDto.FromSqlRaw(sql, parms.ToArray()).ToList();

            string content = string.Empty;

            // Elaboracion de la tabla de Mediciones
            try
            {
                foreach (ParametroMedicionInconsistenciaDto parametroMedicionDto in data)
                {
                    content +=
                    @$"<tr>
					    <td style=""border: 1px solid #000;text-align: center"">
						    {parametroMedicionDto.Medicion}
					    </td>
					    <td style=""border: 1px solid #000;text-align: center"">
						    {Convert.ToInt32(parametroMedicionDto.CasosAuditados)}
					    </td>
					    <td style=""border: 1px solid #000;text-align: center"">
						    { Convert.ToInt32(parametroMedicionDto.InconsistenciasPorDiagnostico)}
					    </td>
					    <td style=""border: 1px solid #000;text-align: center"">
						    { Convert.ToInt32(parametroMedicionDto.InconsistenciasPorSoportes)}
					    </td>
					    <td style=""border: 1px solid #000;text-align: center"">
						    { parametroMedicionDto.PorcentajeGlosa}
					    </td>
				    </tr>
                    ";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            htmlTemplate = htmlTemplate.Replace(Enumeration.Actas.CierreParamsConstantes.TablaMedInconsistencias, content);
        }


        #region Actas

        public async Task<string> GetTemplateFileActaAperturaHtml()
        {
            try
            {
                //Load Path
                string templatePath = Directory.GetCurrentDirectory() + @"\Templates\Actas\acta_apertura.html";
                string image = Directory.GetCurrentDirectory() + @"\Templates\Actas\encabezado.png";

                //Get Html
                var htmlBody = await File.ReadAllTextAsync(templatePath);

                //Replace Image
                //htmlBody = htmlBody.Replace("encabezado.png", image);
                return htmlBody;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetTemplateFileActaCierreHtml()
        {
            try
            {
                //Load Path
                string templatePath = Directory.GetCurrentDirectory() + @"\Templates\Actas\acta_cierre.html";
                string image = Directory.GetCurrentDirectory() + @"\Templates\Actas\encabezado.png";

                //Get Html
                var htmlBody = await File.ReadAllTextAsync(templatePath);

                //Replace Image
                htmlBody = htmlBody.Replace("encabezado.png", image);
                return htmlBody;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public byte[] ConvertHTMLToPDF2(string file, string orientacion)
        {
            byte[] bytes = null;
            using (var mem = new MemoryStream())
            {

                var converter = new HtmlToPdf();

                var licence = context.ParametrosGenerales.Where(x => x.Id == (int)Enumeration.Parametros.LicenciaKeyPDF).FirstOrDefault();

                GlobalProperties.LicenseKey = licence.Valor;
                if (orientacion.Equals("h"))
                {

                    converter.Options.DisplayFooter = true;
                    converter.Footer.DisplayOnFirstPage = true;
                    converter.Footer.DisplayOnOddPages = true;
                    converter.Footer.DisplayOnEvenPages = true;
                    converter.Footer.Height = 100;

                    //string footerString = (string.Concat(HostingEnvironment.MapPath("~"), "\\Resources\\PlantillaFooter.html"));

                    //PdfHtmlSection footerHtml = new PdfHtmlSection(footerString);
                    //footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    //converter.Footer.Add(footerHtml);
                    converter.Options.MarginTop = 50;
                    //converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                }
                else
                {
                    converter.Options.PdfPageSize = PdfPageSize.Custom;
                    double pageWidth = 216;      // Milimeters
                    double pageHeight = 273;     // Milimeters
                    SizeF size = new SizeF((float)(pageWidth * 72 / 25.4), (float)(pageHeight * 72 / 25.4));

                    converter.Options.PdfPageCustomSize = size;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                }
                var doc = converter.ConvertHtmlString(file);

                //if (seguro)
                //{
                //    doc.Security.UserPassword = clave;
                //}

                doc.Save(mem);
                doc.Close();
                bytes = mem.ToArray();
                mem.Close();
                return bytes;
            }

        }


        public byte[] ConvertHTMLToPDF(string file, string orientacion, string fechaCorte, string cobertura, string acta)
        {
            byte[] bytes = null;
            using (var mem = new MemoryStream())
            {
                
                var converter = new HtmlToPdf();

                var licence = context.ParametrosGenerales.Where(x => x.Id == (int)Enumeration.Parametros.LicenciaKeyPDF).FirstOrDefault();

                #region Header
                // header settings
                converter.Options.DisplayHeader = true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 140;

                string headerId = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff").Replace(":", "").Replace(" ", "").Replace("-", "");

                //Build header html file 
                var htmlHeaderText = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Templates\Actas\header.html");

                htmlHeaderText = htmlHeaderText.Replace("$FechaCorte$", fechaCorte)
                                                .Replace("$Cobertura$", cobertura)
                                                .Replace("$Acta$", acta);


                var pathHeader = (Directory.GetCurrentDirectory() + @"\Templates\Actas\" + headerId +  "header.html");

                EscribirArchivoPlano(pathHeader, htmlHeaderText);

                // add some html content to the header
                PdfHtmlSection headerHtml = new PdfHtmlSection(pathHeader);
                //PdfHtmlSection headerHtml = new PdfHtmlSection(htmlHeaderText, "");

                //headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Header.Add(headerHtml);
                #endregion

                #region Footer

                // footer settings
                converter.Options.DisplayFooter = true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 100;

                // add some html content to the footer

                
                PdfHtmlSection footerHtml = new PdfHtmlSection(Directory.GetCurrentDirectory() + @"\Templates\Actas\footer.html");
                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Footer.Add(footerHtml);

                // page numbers can be added using a PdfTextSection object
                PdfTextSection text = new PdfTextSection(0, 10, "Página: {page_number} de {total_pages}  ", new System.Drawing.Font("Arial", 8));
                text.HorizontalAlign = PdfTextHorizontalAlign.Center;
                converter.Footer.Add(text);

                #endregion

                GlobalProperties.LicenseKey = licence.Valor;
                if (orientacion.Equals("h"))
                {

                    converter.Options.DisplayFooter = true;
                    converter.Footer.DisplayOnFirstPage = true;
                    converter.Footer.DisplayOnOddPages = true;
                    converter.Footer.DisplayOnEvenPages = true;
                    converter.Footer.Height = 100;

                    //string footerString = (string.Concat(HostingEnvironment.MapPath("~"), "\\Resources\\PlantillaFooter.html"));

                    //PdfHtmlSection footerHtml = new PdfHtmlSection(footerString);
                    //footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    //converter.Footer.Add(footerHtml);
                    converter.Options.MarginTop = 50;
                    //converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                }
                else
                {
                    converter.Options.PdfPageSize = PdfPageSize.Custom;
                    double pageWidth = 216;      // Milimeters
                    double pageHeight = 273;     // Milimeters
                    SizeF size = new SizeF((float)(pageWidth * 72 / 25.4), (float)(pageHeight * 72 / 25.4));

                    converter.Options.PdfPageCustomSize = size;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                }
                var doc = converter.ConvertHtmlString(file);
                //if (seguro)
                //{
                //    doc.Security.UserPassword = clave;
                //}

                doc.Save(mem);
                doc.Close();
                bytes = mem.ToArray();
                mem.Close();

                if (File.Exists(pathHeader))
                {
                    try
                    {
                        File.Delete(pathHeader);
                    }
                    catch (Exception ex)
                    {

                        ;
                    }
                }
                return bytes;
            }

        }


        public void EscribirArchivoPlano(string path, string line)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(path, true))
                {
                    outputFile.WriteLine(line);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion
    }
}
