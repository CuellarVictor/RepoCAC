import { Component, OnInit } from '@angular/core';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';

@Component({
  selector: 'app-letf-bar',
  templateUrl: './letf-bar.component.html',
  styleUrls: ['./letf-bar.component.scss']
})
export class LetfBarComponent implements OnInit {

  constructor(
    public permisos: GetPermisosServices
  ) { }

  ngOnInit(): void {
  }

}
