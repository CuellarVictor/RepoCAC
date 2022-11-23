import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

//Models
import { ResponseValidacionEstadoModel } from 'src/app/model/registroauditoria/responsevalidacionstado.model';
import { UsuarioLiderModel } from 'src/app/model/registroauditoria/usuariolider.model';
import { RequestConsultaLog } from 'src/app/model/registroauditorialog/requestconsulta.model';
import { RegistroAuditoriaDetalleErrorModel } from 'src/app/model/registroauditoria/registorauditoriadetalleerror.model';
import { CatalogoCoberturaRequestModel } from 'src/app/model/registroauditoria/catalogocoberturarequest.model';
import { HallazgoModel } from 'src/app/model/hallazgos/hallazgos.model';


@Injectable({
  providedIn: 'root'
})
export class CronogramaService {

  constructor(private http: HttpClient) { }
 
  GetLiderIssues(id:any): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GetIssuesLider+'/'+id)
  };

  GetListStates(): Observable<any>{
    return this.http.get<any>(environment.url + environment.EstadosRegistroAuditoria)
  }

  postFiltros(idAuditor:any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoria + environment.RegistrosAuditoriaFiltros, {idAuditor:  idAuditor});
  };

  postDataTable(obj:any): Observable<any>{   
    return this.http.post<any>(environment.url + environment.RegistrosAuditoria + environment.RegistrosAuditoriaFiltrado, obj)
  };

  
  exportToExcel(obj:any): Observable<any>{   
    return this.http.post<any>(environment.url + environment.ExportToExcel  +'/Generate', obj)
  };

  consultaLogAccion(objrequest:RequestConsultaLog): Observable<any>{   
    return this.http.post<any>(environment.url +'/RegistrosAuditoriaLog/ConsultaLogAccion',objrequest)
  };

  getUrlReporteLogAccion(objrequest:RequestConsultaLog): Observable<any>{   
    return this.http.post<any>(environment.url +'/GenerateExcel/GeneraFileLogAccion',objrequest)
  };

  getFileReporteLogAccion(url:string): Observable<any>{   
    return this.http.get<any>(environment.url +'/GenerateExcel/Download/'+url)
  };

  download(fileName:string): Observable<any>{   
    return this.http.get<any>(environment.url + environment.ExportToExcel+'/Download/'+fileName)
  };

  postDataDetalleAsignacion(idAuditor:string[], auditorLider:string): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoria + environment.RegistrosAuditoriaDetallesAsignacion, {idAuditor:  idAuditor, auditorLider});
  };

  postProgresoDiario(idAuditor:any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoria + environment.RegistrosAuditoriaProgresoDiario, idAuditor)
  }

  GetAuditoriaDetalle(id:any): Observable<any>{
    return this.http.get<any>(environment.url + environment.RegistrosAuditoriaDetalle + environment.GetById+'/'+id)
  }

  postGuardarObservacion(data:any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoriaDetalleSeguimiento, data)
  }

  consultaDataTablasReferencialRegistroAuditoria(id:any): Observable<any>{
    return this.http.get<any>(environment.url + '/RegistrosAuditoriaDetalle/ConsultaDataTablasReferencialRegistroAuditoria/'+id)
  }

  getObservacionTemporal(id : string): Observable<any>{
    return this.http.get<any>(environment.url + '/RegistrosAuditoriaDetalleSeguimiento/ConsultaObservacionTemporal/'+id)
  }

  setObservacionTemporal( data : any): Observable<any>{
    return this.http.post<any>(environment.url + '/RegistrosAuditoriaDetalleSeguimiento/RegistraObservacionTemporal', data)
  }
  

  postReversarAuditoriaObservacion(data:any): Observable<any>{
    return this.http.post<any>(environment.url + '/RegistrosAuditoria/ReversarRegistrosAuditoria' , data)
  }

  updateSatusRadioButton(registroauditoriaId:number, userId: string, motivo:any, dato_DC_NC_ND:any, registrosAuditoriaDetalle:any, buttonAction: number): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoriaDetalle + environment.ActualizarDC_NC_ND_Motivo+'/'+buttonAction, 
      {
        registroauditoriaId: registroauditoriaId,
        userId: userId,
        registrosAuditoriaDetalle:registrosAuditoriaDetalle,
        motivoVariable:motivo == null ? "" : motivo,
        dato_DC_NC_ND:dato_DC_NC_ND
      })
  }

  getAutocompleteCatalogoCobertura(data:CatalogoCoberturaRequestModel){
    return this.http.post<any>(environment.url + '/CatalogoCobertura/GetAutocompleteCatalogoCobertura', data)
  }



  closeAuditoria(registroAuditoriaId:any, idEstadoNuevo:any, idUser:any):Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoria + environment.CambiarEstadoRegistroAuditoria, {
      registroAuditoriaId: registroAuditoriaId,
      proceso: "Proces Test",
      observacion: "OBS Test",
      estadoAnterioId: 1,
      estadoActual: idEstadoNuevo,
      asignadoA: "388fcf0e-0544-4ff6-9d89-20c50a1edee8",
      asingadoPor: "f8a7a753-2c97-4ce9-bf42-2b3936606719",
      createdBy: idUser    
    })
  }

  closeAuditoriaMasivo(registroAuditoriaId:any, idEstadoNuevo:any, listadoCalificaciones:any,idUser:any):Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoria + environment.CambiarEstadoRegistroAuditoriaMasivo, {
      registroAuditoriaId:registroAuditoriaId,      
      proceso: "Proces Test",
      observacion: "OBS Test",
      estadoAnterioId: 1,
      estadoActual: idEstadoNuevo,
      asignadoA: "388fcf0e-0544-4ff6-9d89-20c50a1edee8",
      asingadoPor: "f8a7a753-2c97-4ce9-bf42-2b3936606719",
      createdBy: idUser,
      listadoCalificaciones:listadoCalificaciones})
  }

  getOptionsObservation(idCatalogo:any):Observable<any>{    
    // return this.http.get<any>(environment.url + environment.Item + environment.GetByCatalogId ​+'/'+idCatalogo)
    return this.http.get<any>( environment.url + '/Item/GetByCatalogId/'+idCatalogo)    
  }

  // getAuditores(rolId:any, userId:any):Observable<any>{
  //   return this.http.post<any>(environment.url + environment.RegistrosAuditoriaDetalle + environment.GetUsuariosByRoleId, {roleId: rolId,userId: userId})
  // }

  consultaUsuariosLider(userLiderId:any):Observable<any>{
    let url = environment.url + "/RegistrosAuditoria/ConsultaUsuariosLider/"+ userLiderId;
    return this.http.get<UsuarioLiderModel[]>(url)  
  }

  GetCalificacionEsCompletas(registrosAuditoriaId:number):Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistroAuditoriaCalificaciones + environment.GetCalificacionEsCompletas, {registrosAuditoriaId: registrosAuditoriaId})
  }

  getEstadosRegistros(idCatalogo:number): Observable<any>{
    return this.http.get<any>( environment.url + '/Item/GetByCatalogId/' + idCatalogo);
  }   

  //Consulta modelo de validaciones para registroauditoria detalle
  GetValidacionesRegistroAuditoriaDetalle(registroAuditoriaId:number, userId: string, buttonAction: number): Observable<ResponseValidacionEstadoModel>{
    let url = environment.url + "/RegistrosAuditoriaDetalle/GetValidacionesRegistroAuditoriaDetalle/"+ registroAuditoriaId + '/'+ userId + '/' + buttonAction;
    return this.http.get<ResponseValidacionEstadoModel>(url)
  }

  //Actualiza estado registro auditoria y registra seguimiento
  ActualizaEstadoRegistroAuditoria(registroAuditoriaId:number, userId: string, buttonAction: number): Observable<ResponseValidacionEstadoModel>{
    let url = environment.url + "/RegistrosAuditoriaDetalle/ActualizaEstadoRegistroAuditoria/"+ registroAuditoriaId + '/'+ userId+ '/' + buttonAction;
    return this.http.get<ResponseValidacionEstadoModel>(url)
  }

  //Actualiza estado registro auditoria detalle de forma masiva
  ActualizarRegistroAuditoriaDetalleMultiple(registroAuditoriaId:number, userId: string, buttonAction: number, input: any): Observable<boolean>{
    let url = environment.url + "/RegistrosAuditoriaDetalle/ActualizarRegistroAuditoriaDetalleMultiple/"+ registroAuditoriaId + '/'+ userId+ '/' + buttonAction;
    return this.http.post<boolean>(url, input);
  }

  //Actualiza estado registro auditoria detalle de forma masiva
  ValidarErrores(userId: string, registroAuditoriaId:number, input: any): Observable<any>{
    let url = environment.url + "/RegistrosAuditoriaDetalle/ValidarErrores/"+ userId + "/" + registroAuditoriaId;
    return this.http.post<any>(url, input);
  }

  //Consulta acciones lider
  GetConsultaPerfilAccion(): Observable<any>{
    let url = environment.url + "/RegistrosAuditoria/GetConsultaPerfilAccion";
    return this.http.post<any>(url,{})
  }

  //Consulta erorres por id de registro auditoria
  ConsultarErroresRegistrosAuditoria(registroAuditoriaId:number): Observable<RegistroAuditoriaDetalleErrorModel[]>{
    let url = environment.url + "/RegistrosAuditoriaDetalle/ConsultarErroresRegistrosAuditoria/"+ registroAuditoriaId;
    return this.http.get<RegistroAuditoriaDetalleErrorModel[]>(url)
  }

  //Crea o Acutaliza registro auditoria error
  UpsertErroresRegistrosAuditoria(input:RegistroAuditoriaDetalleErrorModel): Observable<boolean>{
    let url = environment.url + "/RegistrosAuditoriaDetalle/UpsertErroresRegistrosAuditoria";
    return this.http.post<boolean>(url, input);
  }

  UpsertErroresRegistrosAuditoriaMasivo(userId: string, obj:any): Observable<any>{
    let url = environment.url + "/RegistrosAuditoriaDetalle/UpsertErroresRegistrosAuditoriaMasivo?userId="+userId;
    return this.http.post<any>(url,obj)
  }


  //Hallazgos
  ConsultarHallazgos(id:any): Observable<any>{
    let url = environment.url + "/Hallazgos/ConsultarHallazgosByRadicadoId";
    return this.http.post<HallazgoModel[]>(url,
      {
        idRadicado: id.toString()
      })
  }

  serviceCalculadoraTFG(obj: any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoriaDetalle + '/CalculadoraTFG', 
      {
        edad: Number(obj.edad),
        hombre: obj.hombre  ,
        creatinina: Number(obj.creatinina),
        peso: Number(obj.peso),
        estatura: Number(obj.estatura)      
      })
  }

  serviceCalculadoraKRU(obj: any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoriaDetalle + '/CalculadoraKRU', 
      {      
        hemodialisis: obj.hemodialisis,
        nitrogenoUrinario: Number(obj.nitrogenoUrinario),
        volumenUrinario: Number(obj.volumenUrinario),
        brunPre: Number(obj.brunPre),
        brunPost: Number(obj.brunPost)      
      })
  }

  serviceCalculadoraPromedio(obj: any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoriaDetalle + '/CalculadoraPromedio', 
     obj)
  }
  

}
