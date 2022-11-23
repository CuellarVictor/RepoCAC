import { Component, OnInit } from '@angular/core';

export interface data {
  id: number;
  value: string;
}

const selector: data[] = [
  {id: 1, value: 'Variable 1'},
  {id: 2, value: 'Variable 2'},
  {id: 3, value: 'Variable 3'},
  {id: 4, value: 'Variable 4'},
]


@Component({
  selector: 'app-detalle-glosa',
  templateUrl: './detalle-glosa.component.html',
  styleUrls: ['./detalle-glosa.component.scss']
})
export class DetalleGlosaComponent implements OnInit {
  type_selector : data[] = selector;
  constructor() { }
  
  ngOnInit(): void {
  }

}
