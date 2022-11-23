import {Component, Inject, OnInit} from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { enumCalculadora } from 'src/app/model/Util/enumerations.enum';
import { CronogramaService } from '../../cronograma/services/cronograma.service';


export interface DialogData {
  id: string;
  tipoCalculadora: string;
}
export interface data {
  id: boolean;
  value: string; 
}


const genero: data[] = [

  {id: true, value: 'Masculino'},
  {id: false, value: 'Femenino'},
];

const hemodialisis : data[] = [
  {id: true, value: 'Hemodiálisis'},
  {id: false, value: 'Diálisis Peritoneal '},
]

@Component({
  selector: 'app-dialog-calculadora',
  templateUrl: './dialog_calculadora.component.html',
  styleUrls: ['./dialog_calculadora.component.scss']
})



export class DialogCalculadoraComponent implements OnInit {


  calculadoraTFG !: FormGroup;
  calculadoraKRU !: FormGroup;
  promedio !: FormGroup;
  type_genero : data[] = genero;
  type_hemodialisis: data[] = hemodialisis;
  resultado = '';
  listaPromedio = ['',''];

  name= '';

  _enumCalculadora : any =  enumCalculadora;

  constructor(private formBuilder: FormBuilder,
    private service : CronogramaService,
    public dialogRef: MatDialogRef<DialogCalculadoraComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {
      switch(this.data.tipoCalculadora){
        case this._enumCalculadora.calculadoraTFG:
          this.name= 'TFG';
          break;
        case this._enumCalculadora.calculadoraKRU:
          this.name = 'KRU';
        break;
        case this._enumCalculadora.promedio:
          this.name = 'Promedio'
          break;
      }
    }

   


    ngOnInit(): void {

   

      this.calculadoraTFG = this.formBuilder.group({
        edad: ['', [Validators.required, Validators.pattern('^([0-9]{0,6})')]],
        hombre: ['', [Validators.required]],
        creatinina: ['', [Validators.required, Validators.pattern('^([0-9].{0,6})')]],
        peso: ['', [Validators.required, Validators.pattern('^([0-9]{0,6})')]],
        estatura: ['', [Validators.required,Validators.pattern('^([0-9]{0,6})')]]
      });

      this.calculadoraKRU = this.formBuilder.group({
        hemodialisis: ['', [Validators.required]],
        nitrogenoUrinario: ['', [Validators.required, Validators.pattern('^([0-9]{0,6})')]],
        volumenUrinario: ['', [Validators.required,Validators.pattern('^([0-9]{0,6})')]],
        brunPre: ['', [Validators.required, Validators.pattern('^([0-9].{0,6})')]],
        brunPost: ['', [Validators.required, Validators.pattern('^([0-9].{0,6})')]],
      });
   
      this.promedio = this.formBuilder.group({
        valor0: ['', [Validators.required, Validators.pattern('^([0-9].{0,6})')]],
        valor1: ['', [Validators.required, Validators.pattern('^([0-9].{0,6})')]]
      });
      /*   
      TFG
      "edad": 39,
      "hombre": true,
      "creatinina": 0.76,
      "peso": 84,
      "estatura": 0

      KRU
      "hemodialisis": true,
      "nitrogenoUrinario": 280,
      "volumenUrinario": 800,
      "brunPre": 40,
      "brunPost": 6.6

    */    
    }
    onchangeEdad(){
      if( this.calculadoraTFG.controls.edad.value<18){
        this.calculadoraTFG.controls.peso.setValue(0);
        this.calculadoraTFG.controls.estatura.setValue('');
      }else{
        this.calculadoraTFG.controls.peso.setValue('');
        this.calculadoraTFG.controls.estatura.setValue(0);
      }        
    }
    

    onchangeHemodialisis(){
      if( this.calculadoraKRU.controls.hemodialisis.value){
        this.calculadoraKRU.controls.brunPost.setValue('');
      }else{
        this.calculadoraKRU.controls.brunPost.setValue(0);
      }
    }

    agregar(){
     
      this.promedio.addControl('valor'+this.listaPromedio.length ,  new FormControl('',  [Validators.required, Validators.pattern('^([0-9].{0,3})')]))
      this.listaPromedio.push('');
      console.log(this.listaPromedio.length);
    }

    eliminar(){
      this.promedio.removeControl('valor'+this.listaPromedio.length+1);
      this.listaPromedio.pop();
      
    
    }

    onsubmit(){
      if(this.data.tipoCalculadora==this._enumCalculadora.calculadoraTFG){
        this.service.serviceCalculadoraTFG(this.calculadoraTFG.value).subscribe(response => {
          this.resultado = response.valor;
          console.log(response)
        }, error => {
          this.resultado = '';
        });
      }else if(this.data.tipoCalculadora==this._enumCalculadora.calculadoraKRU){
        this.service.serviceCalculadoraKRU(this.calculadoraKRU.value).subscribe(response => {
          this.resultado = response.valor;
          console.log(response)
        }, error => {
          this.resultado = '';
        });
      }else if(this.data.tipoCalculadora==this._enumCalculadora.promedio){
          let list = [];
          for(let i=0 ; i<this.listaPromedio.length; i++){
              list.push(Number(this.promedio.get('valor'+i)?.value));
          }
          this.service.serviceCalculadoraPromedio(list).subscribe(response => {
            this.resultado = response.valor;
            console.log(response)
          }, error => {
            this.resultado = '';
          });
      }
       
    }


  onNoClick(): void {
    this.dialogRef.close({status: false , value: null});
  }

  enviar(){
    this.dialogRef.close({status: true, value:this.resultado});
  }

  

}