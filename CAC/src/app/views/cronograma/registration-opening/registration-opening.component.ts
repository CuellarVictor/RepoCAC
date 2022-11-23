import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';


export interface Variables {
  reducido: string;
  estado: string;
  detalle: string;
  hallazgo: string;
  error: string;
  seleccion: string;
  dato_reportado: string;
  motivo: string;
}

export interface Registro {
  name: string;
  error: string;
  variables : Variables[];
 
}

const ELEMENT_DATA: Registro[] = [
  {
    name: 'Glosas',
    error: '',
    variables: [{ 
      reducido: 'V1',
      estado:  '',
      detalle: 'Esta es una variable normal , activa y lista para auditar',
      hallazgo:  '',
      error: '',
      seleccion: '1',
      dato_reportado: '104667',
      motivo: '1'},
      { 
        reducido: 'V2',
        estado:  '',
        detalle: 'El paciente pertenece a la población diagnosticada',
        hallazgo:  '',
        error: '',
        seleccion: '1',
        dato_reportado: '104667',
        motivo: '1'}
        ,
        { 
          reducido: 'V2',
          estado:  '',
          detalle: 'El paciente pertenece a la población diagnosticada',
          hallazgo:  '',
          error: '',
          seleccion: '1',
          dato_reportado: '104667',
          motivo: '1'}
          ,
          { 
            reducido: 'V2',
            estado:  '',
            detalle: 'El paciente pertenece a la población diagnosticada',
            hallazgo:  '',
            error: '',
            seleccion: '1',
            dato_reportado: '104667',
            motivo: '2'}
            ,
            { 
              reducido: 'V2',
              estado:  '',
              detalle: 'El paciente pertenece a la población diagnosticada',
              hallazgo:  '',
              error: '',
              seleccion: '1',
              dato_reportado: '104667',
              motivo: '2'}]
    
  },
  {
    name: 'Demografica',
    error: '',
    variables: [{ 
      reducido: 'V1',
      estado:  '',
      detalle: 'Esta es una variable normal , activa y lista para auditar',
      hallazgo:  '',
      error: '',
      seleccion: '1',
      dato_reportado: '104667',
      motivo: '1'},
      { 
        reducido: 'V2',
        estado:  '',
        detalle: 'El paciente pertenece a la población diagnosticada',
        hallazgo:  '',
        error: '',
        seleccion: '1',
        dato_reportado: '104667',
        motivo: '1'}
        ,
        { 
          reducido: 'V2',
          estado:  '',
          detalle: 'El paciente pertenece a la población diagnosticada',
          hallazgo:  '',
          error: '',
          seleccion: '1',
          dato_reportado: '104667',
          motivo: '1'}
          ,
          { 
            reducido: 'V2',
            estado:  '',
            detalle: 'El paciente pertenece a la población diagnosticada',
            hallazgo:  '',
            error: '',
            seleccion: '1',
            dato_reportado: '104667',
            motivo: '1'}
            ,
            { 
              reducido: 'V2',
              estado:  '',
              detalle: 'El paciente pertenece a la población diagnosticada',
              hallazgo:  '',
              error: '',
              seleccion: '1',
              dato_reportado: '104667',
              motivo: '1'}]
    
  },
  {
    name: 'Tratamiento',
    error: '',
    variables: [{ 
      reducido: 'V1',
      estado:  '',
      detalle: 'Esta es una variable normal , activa y lista para auditar',
      hallazgo:  '',
      error: '',
      seleccion: '1',
      dato_reportado: '104667',
      motivo: '1'},
      { 
        reducido: 'V2',
        estado:  '',
        detalle: 'El paciente pertenece a la población diagnosticada',
        hallazgo:  '',
        error: '',
        seleccion: '1',
        dato_reportado: '104667',
        motivo: '1'}
        ,
        { 
          reducido: 'V2',
          estado:  '',
          detalle: 'El paciente pertenece a la población diagnosticada',
          hallazgo:  '',
          error: '',
          seleccion: '1',
          dato_reportado: '104667',
          motivo: '1'}
          ,
          { 
            reducido: 'V2',
            estado:  '',
            detalle: 'El paciente pertenece a la población diagnosticada',
            hallazgo:  '',
            error: '',
            seleccion: '1',
            dato_reportado: '104667',
            motivo: '1'}
            ,
            { 
              reducido: 'V2',
              estado:  '',
              detalle: 'El paciente pertenece a la población diagnosticada',
              hallazgo:  '',
              error: '',
              seleccion: '1',
              dato_reportado: '104667',
              motivo: '1'}]
    
  },
  {
    name: 'Seguimiento',
    error: '',
    variables: [{ 
      reducido: 'V1',
      estado:  '',
      detalle: 'Esta es una variable normal , activa y lista para auditar',
      hallazgo:  '',
      error: '',
      seleccion: '1',
      dato_reportado: '104667',
      motivo: '1'},
      { 
        reducido: 'V2',
        estado:  '',
        detalle: 'El paciente pertenece a la población diagnosticada',
        hallazgo:  '',
        error: '',
        seleccion: '1',
        dato_reportado: '104667',
        motivo: '1'}
        ,
        { 
          reducido: 'V2',
          estado:  '',
          detalle: 'El paciente pertenece a la población diagnosticada',
          hallazgo:  '',
          error: '',
          seleccion: '1',
          dato_reportado: '104667',
          motivo: '1'}
          ,
          { 
            reducido: 'V2',
            estado:  '',
            detalle: 'El paciente pertenece a la población diagnosticada',
            hallazgo:  '',
            error: '',
            seleccion: '1',
            dato_reportado: '104667',
            motivo: '1'}
            ,
            { 
              reducido: 'V2',
              estado:  '',
              detalle: 'El paciente pertenece a la población diagnosticada',
              hallazgo:  '',
              error: '',
              seleccion: '1',
              dato_reportado: '104667',
              motivo: '1'}]
    
  }
];
export interface data {
  id: number;
  value: string;
}


const medicion: data[] = [
  {id: 1, value: 'Variable 1'},
  {id: 2, value: 'Variable 2'},
  {id: 3, value: 'Variable 3'},
  {id: 4, value: 'Variable 4'},
]

@Component({
  selector: 'app-registration-opening',
  templateUrl: './registration-opening.component.html',
  styleUrls: ['./registration-opening.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class RegistrationOpeningComponent implements OnInit {
  type_medicion : data[] = medicion;
  dataSource = ELEMENT_DATA;
  columnsToDisplay = ['name','DatoReportado','Motivo', 'H', 'E', 'select'];
  expandedElement: Registro | null | undefined;
  constructor() { }

  ngOnInit(): void {
  }
  

}
