import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'; 
import { Observable } from 'rxjs';
import { UserModel } from 'src/app/model/auth/usermodel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdministradorService {

  url_GetUsers = "/GestionUsuariosControllercs/GetUsers" 
  url_GetPanelEnfermedadesMadre = "/GestionUsuariosControllercs/GetPanel" 
  url_GetEnfermedades = "/GestionUsuariosControllercs/GetEnfermedades" 
  url_GetRoles = "/GestionUsuariosControllercs/GetRoles" 
  url_GetUserCode = "/GestionUsuariosControllercs/GetUserCode" 
  url_SubmitUser = "/account/create"
  url_GetLog = "/GestionUsuariosControllercs/GetProcessLogFiltrado"
  
  constructor(private http: HttpClient) { }
 
  getUsers(): Observable<any>{
    return this.http.get<any>(environment.url + this.url_GetUsers);
  } 

  upsertUser(user : UserModel): Observable<any>{
    return this.http.post<any>(environment.url + "/GestionUsuariosControllercs/Upsertuser", user);
  }
 
  submitUser(user: any): Observable<any>{
    return this.http.post<any>(environment.url + this.url_SubmitUser, user);
  }

  getPanelEnfermedadesMadre(): Observable<any>{
    return this.http.get<any>(environment.url + this.url_GetPanelEnfermedadesMadre);
  } 

  getRoles(): Observable<any>{
    return this.http.get<any>(environment.url + this.url_GetRoles);
  } 

  getEnfermedades(): Observable<any>{
    return this.http.get<any>(environment.url + this.url_GetEnfermedades);
  } 

  getLog(data: any): Observable<any>{
    return this.http.post<any>(environment.url + this.url_GetLog, data);
  } 



}
