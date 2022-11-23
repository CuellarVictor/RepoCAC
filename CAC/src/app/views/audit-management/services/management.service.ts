import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CargueCalificacionMasiva } from 'src/app/model/proceso/cargueCalificacionMasiva.model';
import { CarguePoblacionModel } from 'src/app/model/proceso/carguepoblacion.model';
import { CurrentProcessModel } from 'src/app/model/proceso/currentprocess.model';
import { environment } from 'src/environments/environment';
import { objCobertura, objFiltro, objPeriodo, objRegistroEstado } from 'src/utils/models/manager/management';
import { objMedicion } from 'src/utils/models/medicion/medicion';

@Injectable({
  providedIn: 'root'
})
export class ManagementService {

  constructor(private http: HttpClient) { }

  getEnfermedadMadres(objCobertura: objCobertura): Observable<any>{
    return this.http.post<any>(environment.urlCAC + environment.Coberturas, objCobertura)
  }

  RegistroXEstado(RegistroXEstado:objRegistroEstado):Observable <any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoria + environment.RegistrosAuditoriaAsignadoAuditorEstado, {
      pageNumber: RegistroXEstado.pageNumber,
      maxRows: 100,//RegistroXEstado.maxRows,
      idAuditor: RegistroXEstado.idAuditor,
      estado: RegistroXEstado.estado})
  }

  getPeriodos(objPeriodo:any):Observable <any>{
    return this.http.post<any>(environment.urlCAC + environment.Periodo, objPeriodo)
  }

  // getFiltros(objFiltro:any):Observable <any>{
  //   return this.http.post<any>(environment.url + environment.Medicion + environment.GetFiltrosBolsaMedicion, objFiltro)
  // }

  getFiltros(objlider:any):Observable <any>{
    return this.http.post<any>(environment.url+'/BolsasMedicion/GetRegistros',objlider)
  }

  getStates():Observable<any>{
    return this.http.get<any>(environment.url+'/Item/GetByCatalogId/5')
  }

  createMedicion(objMedicion:any):Observable<any>{
    return this.http.post<any>(environment.url + environment.Medicion + '/Post', objMedicion)
  }

  getEnfermedadMadreXUsuario(idUsuario:string):Observable<any>{
    return this.http.post<any>(environment.url + environment.Cobertura, {idUsuario:idUsuario})
  }

  getMedicion(idMedicion:number):Observable<any>{
    return this.http.get(environment.url + environment.Medicion+ environment.GetById +'/'+idMedicion)
  }

  editarMedicion(idMedicion:number,objMedicion:objMedicion):Observable<any>{
    return this.http.post<any>(environment.url + environment.Medicion + '/Update/' + idMedicion, objMedicion)
  }

  deleteMedicion(idMedicion:number):Observable<any>{
    return this.http.get(environment.url + environment.Medicion+'/Delete/'+idMedicion)
  }

  duplicarMedicion(obj:any):Observable<any>{
    return this.http.post(environment.url + "/Variables/DuplicarMedicion", obj)
  }

  GetUsuariosConcatByRoleCoberturaId(objUser:any):Observable<any>{
    return this.http.post(environment.url + environment.RegistrosAuditoriaDetalle + environment.GetUsuariosConcatByRoleCoberturaId, objUser)
  }

  CargarArchivoPoblacion(obj:CarguePoblacionModel, idSubgrupo: number):Observable<any>{
    return this.http.post(environment.url + "/cargue/CargarArchivoPoblacion", 
    {
      Usuario:obj.Usuario,
      Medicion:obj.Medicion,
      FileName:obj.FileName,
      FileBase64: obj.FileBase64,
      idSubgrupo: idSubgrupo
    }
    )
  }

  generarTemplateCarguePoblacion(idMedicion:number, idsubgrupo:number): Observable<any>{
    return this.http.get<any>( environment.url + "/cargue/generarTemplateCarguePoblacion" +'/' +idMedicion+'/'+idsubgrupo);
  }   

  getSelectAuditoresAsignacion(idMedicion:any):Observable<any>{
    return this.http.post(environment.url + environment.Medicion + environment.GetUsuariosBolsaMedicionFiltro, {
      medicionId:idMedicion     
    })
  }

  getUsuariosBolsaMedicion(objBolsaMedicion:any):Observable<any>{
    return this.http.post(environment.url + environment.Medicion + environment.GetUsuariosBolsaMedicion, objBolsaMedicion )
  }
  getEstado(id:number): Observable<any>{
    return this.http.get<any>( environment.url +environment.Item + environment.GetById+'/'+id);
  }   
  ​getRegistrosAuditoriaXBolsaMedicionFiltro(obj:any):Observable<any>{
    return this.http.post(environment.url + environment.Medicion + environment.​GetRegistrosAuditoriaXBolsaMedicionFiltro, obj )
  }
  ​getRegistrosAuditoriaXBolsaMedicion(obj:any):Observable<any>{
    return this.http.post(environment.url + environment.Medicion + environment.​GetRegistrosAuditoriaXBolsaMedicion, obj )
  }
  ReasignacionesBolsaEquitativa(obj:any):Observable<any>{
    return this.http.post(environment.url + environment.Medicion + environment.ReasignacionesBolsaEquitativa, obj )
  }
  ReasignacionesBolsaDetallada(obj:any):Observable<any>{
    return this.http.post(environment.url + environment.Medicion + environment.ReasignacionesBolsaDetallada, obj )
  }

  DescargaReporteReasignar(obj:any,enumerador:number):Observable<any>{
    return this.http.post(environment.url + environment.ExportToExcel + environment.GenerateRegistrosAuditorXBolsaMedicion+enumerador, obj )
  }

  GetUsuariosByRol(obj:any){
    return this.http.post(environment.url + environment.Medicion + environment.GetUsuariosByRol, obj )
     
  }

  GetBolsasMedicionXEnfermedadMadre(obj:any){
    return this.http.post(environment.url + environment.Medicion + environment.GetBolsasMedicionXEnfermedadMadre, obj )
     
  }

  GetValidacionEstadoBolsasMedicion(obj:any){
    return this.http.post(environment.url + environment.Medicion + environment.GetValidacionEstadoBolsasMedicion, obj )
  }
  
MoverTodosRegistrosAuditoriaBolsaMedicion(obj:any):Observable<any>{
  return this.http.post(environment.url + environment.Medicion + environment.MoverTodosRegistrosAuditoriaBolsaMedicion, obj )
   
}

MoverAlgunosRegistrosAuditoriaBolsaMedicion(obj:any):Observable<any>{
  return this.http.post(environment.url + environment.Medicion + environment.MoverAlgunosRegistrosAuditoriaBolsaMedicion, obj )
   
}
MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla(obj:any):Observable<any>{
  return this.http.post(environment.url + environment.Medicion + environment.MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla, obj )
   
}

GetConsultaEstructurasCargePoblacion(idMedicion:number):Observable<any>{
  let obj = {
    medicionId: idMedicion.toString()
  }  
  return this.http.post(environment.url + environment.Medicion +'/GetConsultaEstructurasCargePoblacion',obj);
}

generarTemplateEliminarRAuditoria():Observable<any>{
  return this.http.post(environment.url + environment.Medicion +'/EliminarRegistrosAuditoriaPlantilla',null);
}

PostEliminarRegistrosAuditoria(obj: any):Observable<any>{
  return this.http.post(environment.url + environment.Medicion +'/EliminarRegistrosAuditoria', obj);
}

generarTemplateCalificacionMasiva(idMedicion: number): Observable<any>{
  return this.http.get<any>( environment.url + "/cargue/CreaPlantillaCalificacionMasiva" +'/' + idMedicion);
}   

CargarArchivoCalificacionMasiva(obj:CargueCalificacionMasiva):Observable<any>{
  return this.http.post(environment.url + "/cargue/IniciaProcesoCalificacionMasiva",  obj )
}



  // CargarArchivoPoblacion(fileToUpload: File, user: string, IdMedicion: string):Observable<CurrentProcessModel> {
  //   const formData: FormData = new FormData();
  //   formData.append("File", fileToUpload, fileToUpload.name);
  //   formData.set(user, "userprocess");
  //   formData.set(IdMedicion, "medicion");
  //   return this.http.post<CurrentProcessModel>(environment.url + environment.CarguePoblacion, formData);
  // }
 
}
