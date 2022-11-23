import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GeneralServices {

  
  constructor(private http: HttpClient) { }



  GetAll(apicontroller: string): Observable<any>{
    return this.http.get<any>(environment.url + apicontroller);
  } 

  GetById(id:number, apicontroller: string): Observable<any>{
    return this.http.get<any>(environment.url + apicontroller + "/" + id);
  } 

  Insert(inputmodel:any, apicontroller: string): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoriaDetalleSeguimiento + environment.GetObservacionesByRegistroAuditoriaId, {inputmodel});
  }

  Update(inputmodel:any, apicontroller: string): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoriaDetalleSeguimiento + environment.GetObservacionesByRegistroAuditoriaId, {inputmodel});
  }

}
