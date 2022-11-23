import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { LayoutService } from 'src/app/layout/service/layout.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: LayoutService, private router: Router){}

  canActivate(){
     if(this.authService.getCurrenUser()){
        return true;
     }
     else{
      // this.router.navigate(['/Login']);
       return false;
     }
  }

}
