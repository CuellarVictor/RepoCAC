import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';
import { LayoutService } from '../../service/layout.service';

@Component({
  selector: 'app-tollbar',
  templateUrl: './tollbar.component.html',
  styleUrls: ['./tollbar.component.scss']
})
export class TollbarComponent implements OnInit {
 
   //user
   userEncoded: any;
   objUser: any;
   initialName ="";
   userId: any;
   
   constructor(private router: Router,
    public service: LayoutService,
     private cookie: CookieService) { }
     version='';


   ngOnInit(): void {
     this.version=environment.version;
     
     
     this.userEncoded = sessionStorage.getItem("objUser");
     this.objUser = JSON.parse(this.userEncoded);
 
     if(this.objUser.name != undefined && this.objUser.name != null && this.objUser.name.length > 2)
     {
         this.initialName = this.objUser.name.substring(0,1).toUpperCase();
     }

     this.userId = localStorage.getItem("UserId")
   }


  btnPerfil(){
    this.router.navigate(['/perfil'])
  }

  btnlogin () {
    Swal.fire({
      title: 'Cerrar sesión',
      text: '¿Desea cerrar sesión?',
      icon: 'info',
      showCancelButton: true,
      confirmButtonColor: '#a94785',
      confirmButtonText: 'SI',
      cancelButtonText: 'NO'
    }).then((result) => {
      if (result.value) {
          this.cerrarSesion();
      }
    });
};


  cerrarSesion()
  {
    this.service.cerrarSesion(this.userId).subscribe(
      (Response) => {   
        
        localStorage.clear();
        sessionStorage.clear();
        this.cookie.deleteAll();
        window.location.href =  environment.rutaFront + '/#'

      },
      (error) => { 
      }
    );
  }

}
