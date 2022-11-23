using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Core.Core
{
    interface IProcesoCore
    {
        void ExecuteCurrentProcess();
        Task<string> TestProcess(int currentProcessId);

        /// <summary>
        /// Escribir archivo plano
        /// </summary>
        /// <param name="path">ruta</param>
        /// <param name="line">texto</param>
        void EscribirArchivoPlano(string path, string line);
    }
}
