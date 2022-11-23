﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputEliminarRegistrosAuditoriaPlantillaDto
    {
        [Required]
        public int MedicionId { get; set; }
        public string IdUsuario { get; set; }
    }
}
