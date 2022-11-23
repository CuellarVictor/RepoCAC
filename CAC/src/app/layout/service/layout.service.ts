import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { ActualizaPassword } from 'src/app/model/auth/actualizarpassword';
import { environment } from 'src/environments/environment';
import { User } from 'src/utils/models/user/users';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {

  constructor(private http: HttpClient,private coockie: CookieService) { }

  getCurrenUser():boolean{
    let user_string = this.coockie.get('token');
    if (user_string) {
      return true;
    } else {
      return false;
    }
  }

  postCredentials(credentials: User): Observable<any>{
    return this.http.post<any>(environment.url + environment.autentication, credentials);
  }

  validaTokenRecuperacion(token: string): Observable<any>{
    return this.http.get<any>(environment.url + '/account/ValidaTokenRecuperacion/' + token);
  }

  actualizaPassword(credentials: ActualizaPassword): Observable<any>{
    return this.http.post<any>(environment.url + '/account/ActualizaPassword/', credentials);
  }

  recuperarPassword(credentials: User): Observable<any>{
    return this.http.post<any>(environment.url + '/account/RecuperarPassword/', credentials);
  }

  cerrarSesion(userId: string): Observable<any>{
    return this.http.get<any>(environment.url + '/account/CerrarSesion/' + userId);
  }
}
