export const environment = {
  production: false,
  message:"QA",
  url: "http://186.155.227.2/BACK_AUDICAC/api",//PRE
  // url: "http://52.45.7.73:8089/api",//QA
  // url: "http://52.45.7.73:44381/api",//DEV
  // url: "https://localhost:44381/api", //LOCAL
version: "V.2.0.2",
rutaFront: "",
urlFiles: "https://jsonformatter.curiousconcept.com", //Url repositorio de archivos CAC
autentication: "/account/login",
RegistrosAuditoria: "/RegistrosAuditoria",
RegistrosAuditoriaFiltrado: "/RegistrosAuditoriaFiltrado",
ExportToExcel: "/GenerateExcel",
AuditoriaLog: "/RegistrosAuditoriaLog",
RegistrosAuditoriaFiltros: "/RegistrosAuditoriaFiltros",
RegistrosAuditoriaDetallesAsignacion: "/RegistrosAuditoriaDetallesAsignacion",
RegistrosAuditoriaProgresoDiario: "/RegistrosAuditoriaProgresoDiario",
RegistrosAuditoriaDetalle: "/RegistrosAuditoriaDetalle",
GetById:"/GetById",
RegistrosAuditoriaDetalleSeguimiento:"/RegistrosAuditoriaDetalleSeguimiento",
GetObservacionesByRegistroAuditoriaId:"/GetObservacionesByRegistroAuditoriaId",
CambiarEstadoRegistroAuditoria:"/CambiarEstadoRegistroAuditoria",
ActualizarDC_NC_ND_Motivo:"/ActualizarDC_NC_ND_Motivo",
CambiarEstadoRegistroAuditoriaMasivo: "/CambiarEstadoRegistroAuditoriaMasivo",
Item:"/Item",
GetByCatalogId​:"/GetByCatalogId​",
EstadosRegistroAuditoria: "/EstadosRegistroAuditoria",
GetUsuariosByRoleId: "/GetUsuariosByRoleId",
SoportesEntidad: "/SoportesEntidad",
BancoInformacion: "/BancoInformacion",
GetBancoInformacionByPalabraClave: "/GetBancoInformacionByPalabraClave",
GetAlertasRegistrosAuditoria: "/GetAlertasRegistrosAuditoria",
RegistroAuditoriaCalificaciones: "/RegistroAuditoriaCalificaciones", 
GetcalificacionRegistroAuditoria: "/GetcalificacionRegistroAuditoria",
CalificarRegistroAuditoria: "/CalificarRegistroAuditoria",
GetCalificacionesRegistroAuditoriaByItemId: "/GetCalificacionesRegistroAuditoriaByItemId",
GetCalificacionesRegistroAuditoriaByVariableId: "/GetCalificacionesRegistroAuditoriaByVariableId",
GetCalificacionEsCompletas: "/GetCalificacionEsCompletas",
RegistrosAuditoriaAsignadoAuditorEstado: "/RegistrosAuditoriaAsignadoAuditorEstado",
Periodo: "/Periodo",
Medicion: "/Medicion",
GetBolsasMedicionXEnfermedadMadre: "/GetBolsasMedicionXEnfermedadMadre",
MoverTodosRegistrosAuditoriaBolsaMedicion: "/MoverTodosRegistrosAuditoriaBolsaMedicion",
MoverAlgunosRegistrosAuditoriaBolsaMedicion: "/MoverAlgunosRegistrosAuditoriaBolsaMedicion",
MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla: "/MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla",
GetValidacionEstadoBolsasMedicion: "/GetValidacionEstadoBolsasMedicion",
GetFiltrosBolsaMedicion: "/GetFiltrosBolsaMedicion",
Cobertura: "/Cobertura",
GetUsuariosConcatByRoleCoberturaId: "/GetUsuariosConcatByRoleCoberturaId",
GestionUsuariosControllercs: "/GestionUsuariosControllercs",
GetCalalogWhitItems: "/NewCatalogo",
GetCatalogoWhitItemsById: "/GetById/",
AddCatalogoWhitItemsById: "/Agregar",
UpdateCatalogoWhitItemsById: "/Actualizar",
DeleteCatalogoWhitItemsById: "/Eliminar/",
DeleteItem: "/EliminarItem/",
GetItemById:"/GetItemById/",
GetUsers: "/GetUsers",
Update: "/Update",
 //Servicio asignar bolsa
 GetUsuariosBolsaMedicionFiltro: "/GetUsuariosBolsaMedicionFiltro",
 GetUsuariosBolsaMedicion: "/GetUsuariosBolsaMedicion",
 //Servicio reasignar bolsa
 ​GetRegistrosAuditoriaXBolsaMedicionFiltro: "/GetRegistrosAuditoriaXBolsaMedicionFiltros",
 ​GetRegistrosAuditoriaXBolsaMedicion: "/GetRegistrosAuditoriaXBolsaMedicion",
 ReasignacionesBolsaEquitativa: "/ReasignacionesBolsaEquitativa",
 ReasignacionesBolsaDetallada: "/ReasignacionesBolsaDetallada",
 UrlPlantillaReasignacion: "https://proveedoresonline.s3.amazonaws.com/loadapifiles/20220303210132_Plantilla Reasignacion Bolsa Detallada.xlsx",
 GenerateRegistrosAuditorXBolsaMedicion: "/GenerateRegistrosAuditoriaXBolsaMedicion?enumReport=",
 GetUsuariosByRol: "/GetUsuariosByRol",
 //Servicios Detalles Variables - Lider.
ModuloVariables: "/Variables",
CrearVariables: "/CrearVariables",
ActualizarVariable: "/ActualizarVariables",
GetVariablesFiltrado: "/VariablesFiltrado", //GetVariablesFiltrado
//  ActualizarVariablesLider: "/ActualizarVariablesLider",
ActualizarVariablesLider: "/ActualizarVariablesLiderMasivo",
  GetIssuesLider: "/Lider/GetIssuesLeader",

  //Parametros Generales
  BaseParametrosGenerales: "/ParametrosGeneral/",

  //coneccion para servicios de CAC
  urlCAC: "http://52.45.7.73:8093/api",
  Coberturas: "/Coberturas",

  //Cargue de Poblacion
  CarguePoblacion: "/Medicion/CargarArchivoPoblacion",

  //Current Process
  CurrentProcessGetById: "/procesoactual/CurrentProcessGetById",
  ValidationCurrentProcess: "/procesoactual/ValidationCurrentProcess",
  DeleteCurrentProcess:  "/procesoactual/DeleteCurrentProcess",
  UrlPlantillaCargue: "https://proveedoresonline.s3.amazonaws.com/AnalyzerFiles/plantillacargue.csv"

};
