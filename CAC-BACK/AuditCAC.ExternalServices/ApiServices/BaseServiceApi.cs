using AuditCAC.Dal.Entities;
using AuditCAC.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.ExternalServices.ApiServices
{  
	public class BaseServiceApi
	{
        public BaseServiceApi()
        {

        }

        /// <summary>
        /// Internal method to call Api Service 
        /// </summary>
        /// <param name="url">Api url</param>
        /// <returns>Json Response</returns>
        public ResponseHttpResult ExecuteGetApiService(string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                var response = (HttpWebResponse)request.GetResponse();
                var result = new ResponseHttpResult();

                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        result.JsonResponse = sr.ReadToEnd();
                        result.StatusCode = response.StatusCode.ToString();
                    }
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Internal method to call Api Service with authentication
        /// </summary>
        /// <param name="url">Api url</param>
        /// <param name="token">Token</param>
        /// <returns>Json Response</returns>
        public ResponseHttpResult ExecuteGetApiService(string url, string token)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + token);

            var response = (HttpWebResponse)request.GetResponse();
            var result = new ResponseHttpResult();

            using (var stream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    result.JsonResponse = sr.ReadToEnd();
                    result.StatusCode = response.StatusCode.ToString();
                    result.Token = token;
                }
            }
            return result;
        }

        /// <summary>
        /// Internal method to call Api Service POST
        /// </summary>
        /// <param name="url">Api Rest</param>
        /// <param name="jsonrequest">Json request</param>
        /// <returns>Json response</returns>
        public ResponseHttpResult ExecutePostServices(string url, object jsonrequest)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var result = new ResponseHttpResult();

            string json = JsonConvert.SerializeObject(jsonrequest, Formatting.Indented);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseString = streamReader.ReadToEnd();
                result.JsonResponse = responseString;
                result.StatusCode = httpResponse.StatusCode.ToString();
                return result;
            }
        }

        /// <summary>
        /// Internal method to call Api Service POST whit authentication
        /// </summary>
        /// <param name="url">Api url</param>
        /// <param name="jsonrequest">Json Request</param>
        /// <param name="token">Token</param>
        /// <returns>Json Response</returns>
        public ResponseHttpResult ExecutePostServices(string url, object jsonrequest, string token)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + token);
            httpWebRequest.Accept = "application/json";

            var result = new ResponseHttpResult();

            string json = JsonConvert.SerializeObject(jsonrequest, Formatting.Indented);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                //var responseString = streamReader.ReadToEnd();
                //var response = (HttpWebResponse)httpWebRequest.GetResponse();
                //result.JsonResponse = responseString;
                //result.StatusCode = response.StatusCode.ToString();
                //result.Token = token;
                //return result;

                var responseString = streamReader.ReadToEnd();
                result.JsonResponse = responseString;
                result.StatusCode = httpResponse.StatusCode.ToString();
                return result;
            }
        }

        /// <summary>
        /// Generator token in External Api's
        /// </summary>
        /// <param name="url">Service Security URL</param>
        /// <param name="jsonrequest">Credentials</param>
        /// <returns></returns>
        internal ResponseHttpResult TokenGenerate(string url, object jsonrequest)
        {
            return this.ExecutePostServices(url, jsonrequest);
        }

        /// <summary>
        /// Validate token 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string ValidateToken(string token)
        {

            var urlSecurity = ConfigurationManager.AppSettings["UrlApiSecurityToken"];
            var credentials = new RequestGetToken
            {
                User = ConfigurationManager.AppSettings["UrlApiSecurityUser"],
                Password = ConfigurationManager.AppSettings["UrlApiSecurityPassword"]
            };

            var jwthandler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(token))
            {
                var jwttoken = jwthandler.ReadToken(token);

                var expDate = jwttoken.ValidTo;
                if (expDate < DateTime.UtcNow.AddMinutes(1))
                    return this.TokenGenerate(urlSecurity, credentials).JsonResponse.Replace("\"", "");
                else
                    return token;
            }
            return this.TokenGenerate(urlSecurity, credentials).JsonResponse.Replace("\"", "");
        }
    }
}
