using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AuditCAC.Domain.Entities;

namespace AuditCAC.Domain.Helpers
{
    public class Util
    {

        /// <summary>
        /// Realiza consulta de parametro en el listado y valida que este exista
        /// </summary>
        /// <param name="parametersList">Lista de parametros</param>
        /// <param name="enumeration">Enumeracion del parametro</param>
        /// <returns>Valor del Parametro</returns>
        public string GetAndValidateParameter(List<ParametroGeneralModel> parametersList, Enumeration.Parametros enumeration)
        {
            try
            {
                var parameter = parametersList.Where(x => x.Id == (int)enumeration).FirstOrDefault();

                if (parameter != null)
                {
                    return parameter.Valor;
                }
                else
                {
                    throw new ArgumentException("Parametro " + enumeration.ToString() + " no encontrado");
                }                
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// Realiza cargue de archivo en S3
        /// </summary>
        /// <param name="filePath">ruta fisica</param>
        /// <param name="fileName">nombre archivo</param>
        /// <returns></returns>
        public async Task<PutObjectResponse> LoadFileS3(string filePath, string fileName, S3Model s3)
        {
            try
            {
                var client = new AmazonS3Client(s3.S3AccessKeyId,
                   s3.S3SecretAccessKey,
                   RegionEndpoint.GetBySystemName(s3.S3RegionEndpoint));

                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = s3.S3BucketName,
                    Key = fileName,
                    FilePath = filePath,
                    //ContentType = "text/plain",
                    StorageClass = s3.S3StorageClass
                };

                return await client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
        }

        /// <summary>
        /// Escribir archivo plano
        /// </summary>
        /// <param name="path">ruta</param>
        /// <param name="line">texto</param>
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

        /// <summary>
        /// Elimina las Tildes de una cadena ingresada
        /// </summary>
        /// <param name="InpuText">Texto de entrada</param>
        public string EliminarAcentos(string InpuText)
        {
            try
            {
                //var cleanText = Regex.Replace(InpuText.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ;-]+", "");
                var cleanText = Regex.Replace(InpuText.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9\: ;-]+", "");
                return cleanText;
            }
            catch (Exception ex)
            {
                return "";
                throw;
            }
        }

        /// <summary>
        /// Elimina texto adicional no deseado en String de Base64
        /// </summary>
        /// <param name="InpuText">Texto de entrada</param>
        public string EliminarTextoBase64(string InpuText, string SeparadorBase64)
        {
            try
            {
                var ListadoSeparadores = SeparadorBase64.Split('[');
                foreach (var item in ListadoSeparadores)
                {
                    InpuText = InpuText.Replace(item, "");
                    //input.FileBase64 = input.FileBase64.Replace("data:application/vnd.ms-excel;base64,", "");
                    //input.FileBase64 = input.FileBase64.Replace("data:text/csv;base64,", ""); 
                }

                return InpuText;
            }
            catch (Exception ex)
            {
                return "";
                throw;
            }
        }

        #region SEND EMAIL


        /// <summary>
        ///  Envio de meails
        /// </summary>
        /// <param name="emailTo">Email Destino</param>
        /// <param name="emailFrom">Email Origen</param>
        /// <param name="pssEmailFrom"></param>
        /// <param name="emailPort"></param>
        /// <param name="emailHost"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="nameDocument"></param>
        /// <param name="locationDocument"></param>
        /// <param name="withFormatHtml"></param>
        /// <returns></returns>
        public bool SendEmail(string emailTo, string emailFrom, string pssEmailFrom, int emailPort, string emailHost, string subject, string message, string nameDocument, string locationDocument, bool withFormatHtml, string emailCopy)
        {
            try
            {
                if (subject.Length > 140 || emailTo == null || subject == null || message == null)
                {
                    throw new ArgumentException("No cumple lo necesario para el envio.");
                }

                MailMessage msg = new MailMessage();

                //Defome destination
                msg.To.Add(emailTo);
                msg.Subject = subject;
                msg.SubjectEncoding = Encoding.UTF8;

                if (emailCopy != null)
                {
                    msg.Bcc.Add(emailCopy);
                }

                //Define body
                msg.Body = message;
                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = withFormatHtml;

                //Define Origin
                msg.From = new MailAddress(emailFrom);
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(emailFrom, pssEmailFrom);
                client.Port = emailPort;
                client.Host = emailHost;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                // Send File Attachment
                if (locationDocument != null && nameDocument != null)
                {
                    System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                    contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    contentType.Name = nameDocument;
                    msg.Attachments.Add(new Attachment(locationDocument, contentType));
                }

                //Send Message
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
