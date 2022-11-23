import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Observable, Subject, TimeoutError, timer } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { CloseSessionComponent } from './layout/login/closeSession/close-session/close-session.component';
import { environment } from '../environments/environment';

export interface DialogData {
  animal: string;
  name: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  sub!: Observable<number>;
  val!: number;
  reset$ = new Subject();
  inactividadTime: any;
  ngOnDestroy(): void {
    this.refreshTimer();
  }

  constructor( private coockie: CookieService,
               public dialog: MatDialog){
                  }


    openDialog(): void {
      const dialogRef = this.dialog.open(CloseSessionComponent, {
        width: '300px',
        disableClose: true,
        data: {}

      });

      localStorage.setItem("OpenModalLogin", "false");
    }


  ngOnInit(): void {
    this.initialize();
    this.sub.subscribe((val) => (this.val = val));
    this.ValidateToken();
  }



  ValidateToken()
  {
    

    const sleep = (milliseconds:any) => {
      return new Promise(resolve => setTimeout(resolve, milliseconds))
    }
    
    //Cada dos minutos valida token 
    sleep(120000).then(() => {

      
      let prefixUrl = window.location.href;
      let prefixUrlSplit = prefixUrl.split('/');
      let lastPositionUrl = prefixUrlSplit[prefixUrlSplit.length - 1];
      console.log(lastPositionUrl);

      //If This isn´t login
      if(lastPositionUrl != '' && lastPositionUrl != 'Login')
      {
        console.log('This isn´t login');

         // Construye fecha de expiracion consultada en la sesión
        let expirationString = "";    
        let expirationString2 = localStorage.getItem("Expiration") == undefined ? "" :  localStorage.getItem("Expiration")?.toString();
        expirationString = expirationString2 == undefined ? "" : expirationString2;
        let expiration = new Date(expirationString);

        // Le resta 5 minutos a la fecha de expiración
        expiration.setTime(expiration.getTime() - (5 * 60 * 1000));


        //Open Modal
        let openModal = localStorage.getItem("OpenModalLogin") 

        console.log("Interceptor: expiration", expiration);
        console.log("Interceptor: CURRENT", new Date());
        console.log("openModal", openModal);

        if(new Date() >= expiration && openModal == "false")
        {
          console.log("Interceptor: EXPIRED");
          localStorage.setItem("OpenModalLogin", "true");

          this.dialog.open(CloseSessionComponent, { 
            disableClose: true,
            data: {},
          });

       }



      }
      



      this.ValidateToken();
      
      
    });


    // this.dialog.open(CloseSessionComponent, { 
    //   disableClose: true,
    //   data: {},
    // });
  }








  initialize(): void {
    this.sub = this.reset$.pipe(startWith(void 0),switchMap(() => timer(10, 1000)));
    this.inactividadTime = sessionStorage.getItem("inactivityTime");

    this.sub.subscribe(r => {
     // console.log(r);
      if(Number(this.inactividadTime)!=0){
        if(Number(r) == Number(this.inactividadTime) && this.coockie.get('token')){
          this.openDialog();
        }
      }     
    })
  }

  refreshTimer(): void {
    this.reset$.next(void 0);
  }

}
