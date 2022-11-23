import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { MatDialog } from '@angular/material/dialog';
import { CloseSessionComponent } from 'src/app/layout/login/closeSession/close-session/close-session.component';
import { ItemModel } from 'src/app/model/Util/item.model';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService implements HttpInterceptor {

  token:any;

  constructor(
    private router: Router,
    private coockie: CookieService,
    public dialog: MatDialog,
    private http: HttpClient
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

     this.token = this.coockie.get('token');



    if(this.token){
      request = request.clone({headers: request.headers.set('Authorization', `Bearer ${ this.token }`)});
    }

    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status == 401) {
          // sessionStorage.clear();
          // localStorage.clear()
          // this.coockie.deleteAll();
           this.router.navigateByUrl('/Login');
          // window.location.reload();
        }
        return throwError( err );
      })
    );

     

    
  }
}
