import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VariableCondicional } from 'src/app/model/Variables/variablecondicionada.model';

import { VariableDetailsService } from 'src/app/views/audit-management/variable-details/services/variable-details.service';
import { InputsVariables } from 'src/utils/models/variable-details/Variables';

@Component({
  selector: 'app-variable-condicionada',
  templateUrl: './variable-condicionada.component.html',
  styleUrls: ['./variable-condicionada.component.scss']
})
export class VariableCondicionadaComponent implements OnInit {


  responseDataTable: any;
  busqueda='';
  //Condicional
  InputsVariables = new InputsVariables();
  idMedicion!: number ;
  idVariable!: number ;
  variableCondicionadaList: VariableCondicional[] = [];
  valorConstanteList: string []=[];
  valorConstante: string = '';
  resultCosultaList: VariableCondicional[] = [];
  activarContinuar = false;
  constructor(@Inject(MAT_DIALOG_DATA) public data:any,
  private serviceVariableDetails: VariableDetailsService) { }

  ngOnInit(): void {
    this.idVariable  = this.data.idVariable;
    this.idMedicion = this.data.idMedicion;
    this.variableCondicionadaList = this.data.variableCondicionadaList;
    this.valorConstante = this.data.valorConstante;
    console.log(this.data.variableCondicionadaList);
    this.getListConstante();
    this.activarBtnContinuarAction();
  }

  getListConstante(){
    if(this.valorConstante!=''){
      this.valorConstanteList = this.valorConstante.split(' | ');
    }
    
  }


  getVariablesFiltrado(search: string) {

    
    this.InputsVariables.medicionId = this.idMedicion.toString();
    this.InputsVariables.maxRows = 5;
    this.InputsVariables.alerta = "";
    this.InputsVariables.variable = search;
    this.InputsVariables.descripcion = search;

    this.resultCosultaList = [];
    this.serviceVariableDetails.GetVariablesFiltrado(this.InputsVariables).subscribe(
        (Response) => {        
          this.resultCosultaList = []; 
          this.responseDataTable = Response;     
          
          this.responseDataTable.data.forEach((element:any) => {
            let item = new VariableCondicional();
            item.variableHijaId = element.id;
            item.nombre = element.nombre;
            item.descripcion = element.descripcion;
            item.enable = false

            if(this.variableCondicionadaList.filter(x => x.variableHijaId ==  item.variableHijaId).length > 0)
            {
              item.enable = true;
            }

            this.resultCosultaList.push(item);
            
          });
        },
        (error) => {
          console.log(error);
        }
      );
  }

  

  AddOrRemoveVariable(variable: VariableCondicional, enable: boolean)
  {
    if(enable) //Add Remove
    {
      this.busqueda='';
      this.getVariablesFiltrado('');
     if(!this.variableCondicionadaList.find(e=> e.nombre==variable.nombre)){
      this.variableCondicionadaList.push(variable);
      this.valorConstanteList.push('');
     }
      
    }
    else{
      let position = this.variableCondicionadaList.indexOf(this.variableCondicionadaList.filter(x => x.variableHijaId == variable.variableHijaId)[0]);
      this.variableCondicionadaList.splice(position, 1);
      this.valorConstanteList.splice(position,1);
    }
  }

  agregarValorConstante(variable: VariableCondicional, constante : string){
    console.log(this.valorConstanteList)
    let position = this.variableCondicionadaList.indexOf(this.variableCondicionadaList.filter(x => x.variableHijaId == variable.variableHijaId)[0]);
    this.valorConstanteList[position]=constante;
    this.activarBtnContinuarAction();
 
  }

  activarBtnContinuarAction(){
    this.valorConstanteList.length==0?this.activarContinuar=false:this.activarContinuar=true;
    this.valorConstanteList.map(e=> e==''?this.activarContinuar=false:null);
  }


  setVariablesCondicional()
  {
    this.serviceVariableDetails.setVariablesCondicional(this.variableCondicionadaList);
    this.valorConstante= '';
    this.valorConstanteList.map((e,i) => {      
      if(i==0){
        this.valorConstante=e;
      }else{
        this.valorConstante=this.valorConstante+' | '+e;
      }
      } );
    this.serviceVariableDetails.setValorConstante(this.valorConstante);
  
  }

}
