import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TabsService {

  constructor(private http: HttpClient) { }

  postGetObservations(IdRegistroAuditoria:any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoriaDetalleSeguimiento + environment.GetObservacionesByRegistroAuditoriaId, {registroAuditoriaId:IdRegistroAuditoria});
  };

  GetDataSupports(IdRegistroAuditoria:any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoria + environment.SoportesEntidad, {idRegistrosAuditoria:IdRegistroAuditoria});
  };

  postDataBankInformation(data:any): Observable<any>{
    return this.http.post<any>(environment.url + environment.BancoInformacion + environment.GetBancoInformacionByPalabraClave, data)
  }

  // Servicio que lista Items y sus calificaciones en caso de tenerlas.
  getQualifications(registrosAuditoriaId:any, variableId:any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistroAuditoriaCalificaciones + environment.GetCalificacionesRegistroAuditoriaByVariableId, {registrosAuditoriaId: registrosAuditoriaId, variableId: variableId})  
  }

  // Original, va directo a catalogos y filtra segun su CatalogId
  // getCatalogCalifications(): Observable<any>{
  //   return this.http.get<any>(environment.url+'/Item/GetByCatalogId/3')
  // }  

  // Nuevo, consulta Items pertenecnientes a la Variable seleccionada.  
  getCatalogCalifications(VariableId:number): Observable<any>{
    return this.http.get<any>(environment.url + '/RegistroAuditoriaCalificaciones/GetItemsByVariableId' + "/" + VariableId);
  } 

  saveCalification(calification:any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistroAuditoriaCalificaciones + environment.CalificarRegistroAuditoria, calification);
  }

}
