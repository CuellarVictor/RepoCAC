using AuditCAC.Core.Core;
using AuditCAC.Dal.Data;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Bash
{
    public class BashCore
    {
        public void StartBashProcess(string pahtLog, S3Model s3)
        {
            DBAuditCACContext _dBAuditCACContext = new DBAuditCACContext();

            ProcesoCore core = new ProcesoCore(_dBAuditCACContext, s3, pahtLog);

            core.ExecuteCurrentProcess();

        }

        #region Write

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

        #endregion
    }
}
