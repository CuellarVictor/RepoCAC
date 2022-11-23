import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class genericService {

  constructor(private http: HttpClient) { }

  postDate(idUsuario:any): Observable<any>{
    return this.http.post<any>(environment.url + environment.RegistrosAuditoria + environment.GetAlertasRegistrosAuditoria, {idUsuario: idUsuario})
  } 
}
