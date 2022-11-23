export class Registro {
  name!: string;
  error!: string;
  variables!: Variables[];
}

export class Variables {
  reducido!: string;
  estado!: string;
  detalle!: string;
  hallazgo!: string;
  error!: string[];
  seleccion!: string;
  dato_reportado!: string;
  motivo!: string;
  registroAuditoriaDetalleId: number = 5;
}

export class detalleRegistro{
activo!: boolean
ara!: boolean
araAtendido!: boolean
conclusion!: string
createdBy!: string
createdDate!: string
displayOrder!: string
entidad!: string
encuesta!: boolean
eps!: boolean
epsAtendido!: boolean
estado!: string
estadoCodigo!: string
estadoNombre!: string
extemporaneo!: boolean
fechaAsignacion!: string
fechaAuditoria!: string
fechaCreacion!: string
fechaMaxConsultaAudit!: string
fechaMinConsultaAudit!: string
fechaNacimiento!: string
fechaReverso!: string
id!: string
idAuditor!: string
idEntidad!: string
idMedicion!: string
idPeriodo!: string
idRadicado!: string
identificacion!: string
modifyBy!: string
modifyDate!: string
nombreMedicion!: string
fechaCorteAuditoria!: string
primerApellido!: string
primerNombre!: string
reverse!: boolean
revisar!: boolean
segundoApellido!: string
segundoNombre!: string
sexo!: string
tipoIdentificacion!: string
urlSoportes!: string
levantarGlosa!: number
mantenerCalificacion!: number
comiteAdministrativo!: number
comiteExperto!: number
nombreEPS:any
enfermedadMadre!:string
estadoMedicion!: number
}

