export enum enumParametrosGenerales {
    URLAuditCACCoreCACServices = 1,
    UsuarioTokenAuditCACCoreCACServices = 2,
    PasswordTokenAuditCACCoreCACServices = 3,
    InactivityTime = 7,
    ExpirationTime = 8,
    SessionDead = 9,
    Autenticaciónpordirectorioactivo = 10,
    GruposDetalle = 11,
}

export class requestBase {
    GetAll:string = "GetAll";
    GetById = "GetById";
    Insert = "Insert";
    Update = "Update";
}

export enum enumRespuestaLogin
{
    Exitoso = 1,
    UsuarioNoExiste = 2,
    ContrasenaIncorrecta = 3,
    UsuarioBloqueadoPorIntentosFallidos = 4,
    UsuarioInactivo = 5,
    ActualizarContrasena = 6,
    SesionActiva = 7
}

export enum enumProcesos 
{
    CarguePoblacion = 1,
    CalificacionMasiva = 2
}

export enum enumValidationProcess {
    Initializing = -1,
    Finish = -2,
    Error = -3,
    NotFound = -4,
    InProgress = 0,
  }

export enum enumCatalog{
    CalificacionDefecto = 6,
    EstadoUsuario = 16,
    Actas = 22,
    TipoInputActas = 23,
    TipoDatoActas = 24
}

export enum enumEstadoRegistroAuditoria
{
    Registronuevo = 1,
    Glosaenrevisionporlaentidad = 2,
    Glosaobjetada1	= 3,
    Glosaobjetadaenrevisionporlaentidad = 4,
    Glosaobjetada2 = 5,
    Errorlogicamarcacionauditor = 6,
    Errorlogicamarcacionlíder = 7,
    Enviadoaentidad = 8,
    Comiteadministrativo = 9,
    Comiteexpertos = 10,
    Respuestaadministrativa = 11,
    Hallazgo1 =	12,
    Hallazgo1enviadoaentidad = 13,
    Hallazgo2lider = 14,
    Hallazgo2auditor = 15,
    Registrocerrado = 16,
    Registropendiente = 17,
}

export enum enumActionRegistroAuditoriaDetalle
{
    Editar = 1,
    LevantarGlosa = 2,
    MantenerCalificacion = 3,
    Comiteadministrativo = 4,
    Comiteexperto = 5,
    ErroresEncontrados = 6,
}

export enum enumRoles
{
    Admin = 1,
    Lider = 2,
    Auditor = 3
}

export enum enumRolesNombre
{
    Auditor= 'Auditor medico'
}

export enum enumTipoObservacion
{
    General = 1,
    Glosa = 2,
    Glosaobjetada1 = 3,
    Errorlogica = 4,
    Hallazgo = 6,
    Hallazgo2 = 7,
    ComiteAdministrativo = 8,
    ComiteExperto = 9,
    Respuestacomite = 10,
    Cambiodeestado = 11,
    Glosaobjetada2 = 12,
    Levantarglosa = 13,
    Mantenercalificacion = 14,
}

export enum enumTipoVariable
{
    Resolucion = 35,
    Informativa = 36,
    Adicional = 37,
    Glosa = 77
}

export enum enumGrupoVariable
{
    Glosas	=	24	,
    Demográfica	=	25	,
    Tratamiento	=	26	,
    Seguimiento	=	27	,
    Singrupo	=	72	,
    Variablesocultas	=	74	,
}


export class messageString {
    //General
    ErrorMessage:string = "Lo sentimos, se presento un error. Por favor intente nuevamente";
    SuccessMessage:string = "Proceso realizado exitosamente";
    ObligatoryField: string = "Campo obligatorio";
    ObligatoryForm: string = "Por favor diligenciar los campos obligatorios";
    ObligatoryPassword: string = "Por favor diligenciar la constraseña";
    ObligatoryAssingCobertura: string = "Por favor seleccionar las enfermedades asignadas al lider";

    //Audit register detail
    NecesaryObservationMessage:string = "Por favor registre la observación"
    NecesaryIPSCalificationMessage:string = "Por favor registre la calificación de IPS"
    MaintainQualification: string = "¿Está seguro que desea mantener la calificación de la variable?"
    UpGlosa: string = "¿Está seguro que desea levantar la glosa?"
    ComiteManagement: string = "Lider, confirmando esta acción el registro pasará a estado de Comité y deberá compartir el veredicto en las observaciones una vez lo tenga ¿Está seguro?"
    ComiteExpert: string = "Lider, confirmando esta acción el registro pasará a estado de Comité y deberá compartir el veredicto en las observaciones una vez lo tenga ¿Está seguro?"
    MotiveEmpty: string = "Por favor ingrese el motivo de la variables No conforme NC \n o  \n de las variables adicionales en dato conforme DC"
    NecesaryObservationErrorTitle:string = "El registro marca errores en variables que aún no han sido reportados"
    NecesaryObservationErrorMessage:string = "Revise si pertenece a errores que puede corregir, en caso contrario debería reportarlos al lider"
    NecesaryObservationErrorLiderMessage:string = "Por favor valide los errores reportados por el auditor"
    ErroresCorregiblesTitle:string = "Algunas variables han sido marcadas con errores de logica"
    ErroresCorregiblesMessage:string = "Revise los errores corregibles (marcados en rojo) que han sido validados por el lider"
    CoberturaNotFoundLider:string = "La cobertura seleccionada no tiene lider asignado"
    ApplyConditionalVariable: string = "La variable {{varPadre}}  condiciona las variables {{varHija}} para que tome el valor ({{valorCondicionado}}), ¿Desea aplicar los cambios?"
    NecesaryUserMessage:string = "Por favor ingrese el usuario"



}

export enum ReporteMedicion
{
    ReasignacionBolsaData = 1,
    ReasignacionTotalData = 2,
    ReasignacionBolsaPlantilla = 3,

}
export enum EstadosBolsa
{
    Creada = 'Creada',
    En_Curso = 'En curso',
    Asignada = 'Asignada',
    Finalizada = 'Finalizada'
}

export enum  estructuraMedicion
{
   Estructura1 = 39,
   Estructura2 = 40,
   Estructura3 = 41
}

export enum enumTipoCampo
{
    Numerico = 86,
    Alfanumerico = 87,
    Fecha = 88,
    Decimal = 89,
}

export enum enumCalificacion
{
    DC = 32,
    NC = 33,
    ND = 34,
}

export enum enumCatalogo{
    Tipo = 2
}

export enum enumValidacionToken{
    Correcto = 1,
	invalida = 2,
	utilizada = 3,
	Caducada = 4
}

export enum enumPermisos{
    
    Gestor_de_Mediciones	=	1	,
    Gestor_de_Usuarios	=	2	,
    Cronograma	=	3	,
    Configuracion	=	4	,
    Cargar_Poblacion	=	5	,
    Mover_Registros	=	6	,
    Asignacion	=	7	,
    Editar_Medicion	=	8	,
    Duplicar_Medicion	=	9	,
    Ver_Variables	=	10	,
    Eliminar_Medicion	=	11	,
    Nueva_medicion	=	12	,
    Crear_Variable	=	13	,
    Editar_Variable	=	14	,
    Nuevo_usuario	=	15	,
    Editar_Usuario	=	16	,
    Inactivar_Usuario	=	17	,
    Eliminar_Usuario	=	18	,
    Log_Usuario	=	19	,
    Parametrizacion_calificacion_IPS	=	20	,
    Agregar_nuevo_Item_IPS	=	21	,
    Grupo__de_Variables	=	22	,
    Agregar_nuevo_item_Grupos_de_Variables	=	23	,
    Administracion_de_Catalogos	=	24	,
    Nuevo_Catalogo	=	25	,
    Items	=	26	,
    Eliminar_Catalogo	=	27	,
    Editar_Catalogo	=	28	,
    Crear_Item_Cobertura	=	29	,
    Eliminar_Item_Cobertura	=	30	,
    Editar_Item_Cobertura	=	31	,
    Codigos_administrativos	=	32	,
    Crear_Codigo_administrativo	=	33	,
    Editar_Codigo_administrativo	=	34	,
    Eliminar_Codigo_administrativo	=	35	,
    Cargar_plantilla_codigos_administrativos	=	36	,
    Auditoria	=	37	,
    Errores	=	38	,
    Soportes	=	39	,
    Detalles_del_registro_en_auditoria	=	40	,
    CalificacionIPS	=	41	,
    Banco_de_Informacion	=	42	,
    Comite_adminsitrativo	=	43	,
    Comite_Experto	=	44	,
    Mantener_calificacion	=	45	,
    Levantar_Glosa	=	46	,
    Editar	=	47	,
    Guardar	=	48	,
    Administracion_rol = 49 ,
    Tipo_codigos_administrativo_rol = 50 ,
    Eliminar_Variable = 51,
    Eliminar_Registro_Auditoria = 52,
    Consulta_Asignacion_lider_entidad = 53,
    Asignar_auditor_lider = 54,
    Calificacion_Masiva = 55,
    Permisos_Accesos = 56,
    Modificar_Fecha_Final_Medicion = 57,
    Generar_Actas = 58,
    Parametrizar_Actas = 59
}


export enum enumCalculadora{
    calculadoraTFG = 134 ,
    calculadoraKRU = 135 ,
    promedio = 136
}

export enum enumEstadosMedicion {
    En_curso =   28 ,
    Asignada  =  29,
    Finalizada  =  30,
    Creada  =  31
}

export enum enumTipoInputActas {
    Parametrizacion =   140 ,
    Usuario  =  141,
    Consultado  =  142
}

export enum enumTipoDatoActas{
    Texto =   143 ,
    Listado  =  144
}




