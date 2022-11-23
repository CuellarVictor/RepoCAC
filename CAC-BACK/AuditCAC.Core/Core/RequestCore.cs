using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.ExternalServices.ApiServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Core.Core
{
    public class RequestCore : IRequestCore
    {
        private BaseServiceApi _requestBaseApi;
        DBAuditCACContext _dBAuditCACContext;


        public ResponseApiServiceResult requestAuthenticationCAC(string urlBase, string email, string password)
        {
            try
            {
                var autorizacion = new
                {
                    email = email,
                    password = password
                };

                _requestBaseApi = new BaseServiceApi();

                var query = _requestBaseApi.ExecutePostServices(urlBase + "api/account/login", autorizacion);

                var bodyObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenModel>(query.JsonResponse);

                var result = new ResponseApiServiceResult
                {
                    HttpStatusCode = query.StatusCode,
                    Result = bodyObject,
                    Token = bodyObject.token
                };

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            throw new NotImplementedException();
        }

        public ResponseApiServiceResult requestGetpatient(string url, object Data, string token)
        {
            try
            {
                _requestBaseApi = new BaseServiceApi();
                var query =_requestBaseApi.ExecutePostServices(url + "api/MoverRegistrosPascientes/ConsultarRegistrosAuditoriaPascientes", Data, token);
                var bodyObject = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(query.JsonResponse);
                var result = new ResponseApiServiceResult
                {
                    HttpStatusCode = query.StatusCode,
                    Result = bodyObject,
                    //Token = bodyObject.token
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            throw new NotImplementedException();
        }


        public ResponseApiServiceResult requestPostpatient(string url, object Data, string token)
        {
            try
            {
                _requestBaseApi = new BaseServiceApi();
                var query = _requestBaseApi.ExecutePostServices(url + "api/MoverRegistrosPascientes/InsertarRegistrosAuditoriaPascientes", Data, token);
                var bodyObject = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(query.JsonResponse);
                var result = new ResponseApiServiceResult
                {
                    HttpStatusCode = query.StatusCode,
                    Result = bodyObject,
                    //Token = bodyObject.token
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rquest a CAC Core para consultar Nombres y Id de tablas referenciales
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<ResponseGetTablasReferencialDto> requestGetTablasReferencialesPorId(string url, object Data, string token)
        {
            try
            {
                _requestBaseApi = new BaseServiceApi();
                var query = _requestBaseApi.ExecutePostServices(url + "api/TablasReferencial/GetTablasReferencialByValorReferencial", Data, token);
                var bodyObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseGetTablasReferencialDto>>(query.JsonResponse);
                var result = new ResponseApiServiceResult
                {
                    HttpStatusCode = query.StatusCode,
                    Result = bodyObject,
                    //Token = bodyObject.token
                };
                return bodyObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            throw new NotImplementedException();
        }


        /// <summary>
        /// Rquest a CAC Core para consultar las coberturas
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<CoberturaModel> requestGetCoberturas(string url, object Data, string token)
        {
            try
            {
                _requestBaseApi = new BaseServiceApi();
                var query = _requestBaseApi.ExecutePostServices(url + "api/Coberturas", Data, token);
                var bodyObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CoberturaModel>>(query.JsonResponse);
                var result = new ResponseApiServiceResult
                {
                    HttpStatusCode = query.StatusCode,
                    Result = bodyObject,
                    //Token = bodyObject.token
                };
                return bodyObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rquest a CAC Core para consultar las coberturas
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<PeriodoModel> requestGetPeriodos(string url, object Data, string token)
        {
            try
            {
                _requestBaseApi = new BaseServiceApi();
                var query = _requestBaseApi.ExecutePostServices(url + "api/Periodo", Data, token);
                var bodyObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeriodoModel>>(query.JsonResponse);
                var result = new ResponseApiServiceResult
                {
                    HttpStatusCode = query.StatusCode,
                    Result = bodyObject,
                    //Token = bodyObject.token
                };
                return bodyObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            throw new NotImplementedException();
        }


        /// <summary>
        /// Rquest a CAC Core para consultar las Bolsas de Medicion
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<MedicionModel> RequestGetMediciones(string url, object Data, string token)
        {
            try
            {
                _requestBaseApi = new BaseServiceApi();
                var query = _requestBaseApi.ExecutePostServices(url + "api/MedicionAll", Data, token);
                var bodyObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MedicionModel>>(query.JsonResponse);
                var result = new ResponseApiServiceResult
                {
                    HttpStatusCode = query.StatusCode,
                    Result = bodyObject,
                    //Token = bodyObject.token
                };
                return bodyObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            throw new NotImplementedException();
        }





    }
}
