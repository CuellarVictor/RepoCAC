import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CurrentProcessModel } from 'src/app/model/proceso/currentprocess.model';
import { environment } from 'src/environments/environment';
import { objCobertura, objFiltro, objPeriodo, objRegistroEstado } from 'src/utils/models/manager/management';
import { objMedicion } from 'src/utils/models/medicion/medicion';

@Injectable({
  providedIn: 'root'
})
export class ProcessService {

  constructor(private http: HttpClient) { }


  currentProcessGetById(id:number):Observable<any>{
    return this.http.get(environment.url + environment.CurrentProcessGetById+'/'+id)
  }

  validationCurrentProcess(id:number,  result : string):Observable<any>{
    return this.http.get(environment.url + environment.ValidationCurrentProcess+'/'+id+'/'+result)
  }

  deleteCurrentProcess(id:number,  result : string):Observable<any>{
    return this.http.get(environment.url + environment.DeleteCurrentProcess+'/'+id+'/'+result)
  }

 
}
