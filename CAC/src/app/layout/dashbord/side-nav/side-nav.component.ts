import { Component, Input, OnInit } from '@angular/core';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit { 
  
  rol: any;

  constructor(public permisos: GetPermisosServices) {
    permisos.contruir();
   }
  
  ngOnInit(): void {

    this.rol = sessionStorage.getItem("rol");

  }

}
