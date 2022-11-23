import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CookieService } from "ngx-cookie-service";
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import Swal from "sweetalert2";
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { SelectionModel } from "@angular/cdk/collections";
import {map, startWith} from 'rxjs/operators';
import { Observable } from "rxjs/internal/Observable";
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {FormsModule} from '@angular/forms';


//Models
import { objuser } from 'src/utils/models/cronograma/cronograma';
import { CatalogoRequest } from "src/app/model/catalogo/catalogorequest.model";
import { InputsVariables } from "src/utils/models/variable-details/Variables";

//Servicio
import { VariableDetailsService } from '../../../variable-details/services/variable-details.service';
import { ConfigService } from 'src/app/views/config/service/config.service';
import { VariableResponseModel } from 'src/app/model/Variables/variableresponse.model';
import { ItemModel } from 'src/app/model/Util/item.model';
import { VariableCondicionadaComponent } from "../variable-condicionada/variable-condicionada/variable-condicionada.component";
import { VariableCondicional } from "src/app/model/Variables/variablecondicionada.model";
import { ModalVariableComponent } from "../../../modal-variable/modal-variable.component";
import { enumGrupoVariable, enumTipoCampo, enumTipoVariable, EstadosBolsa, messageString } from "src/app/model/Util/enumerations.enum";




@Component({
  selector: 'app-variable-upsert',
  templateUrl: './variable-upsert.component.html',
  styleUrls: ['./variable-upsert.component.scss']
})
export class VariableUpsertComponent implements OnInit {


  //Detalle medicion
  detalleEncoded: any;
  detalleUncoded: any;
  nombreModuloAnterior: any = "";

  //user
  userEncoded: any;
  objUser: objuser;

  //Variable
  modeloVariable:any;
  variableSelected: VariableResponseModel = new VariableResponseModel();
  itemIPSselected: number[] = [];

  //Elementos Iniciales
  InputsVariables = new InputsVariables();
  responseDataTable: any;
  loading: boolean = true;
  tipoVariable: ItemModel[] = [];
  grupoValueComplete: ItemModel[] = [];
  grupoValue: ItemModel[] = [];
  defaultValue: ItemModel[] = [];
  auditable: boolean = false;
  calificaciones: ItemModel[] = [];
  catalogo: ItemModel[] = [];
  condicionVariable: ItemModel[] = [];
  reglasVariable: ItemModel[] = [];
  tipoCampoList: ItemModel[] = [];
  tipoCalculadoraList: ItemModel[] = [];
  idMedicion: number = 0;

  //Catalogo Cobertura
  objFiltro : CatalogoRequest = new CatalogoRequest();
  resultCatalogList: any;
  teststring : any;


  //Autocomplete
  options!: any[] ;
  filteredOptions!: Observable<string[]>;

  //Condicionales
  objCondicional:any;
  variableCondicionadaList: VariableCondicional[] = [];
  textoVariablesCondicionada: string = "";  

  //Enumerations
  _messageString: messageString = new messageString();
  _enumTipoVariable: any = enumTipoVariable;
  _enumTipoCampo: any = enumTipoCampo;

  //Validations
  correctTipoVariable : boolean = true;
  correctNombreVariable : boolean = true
  correctOrdenVariable : boolean = true;
  correctCalificacionDefaultVariable : boolean = true;
  correctGrupoVariable : boolean = true;
  correctDescripcionVariable : boolean = true;
  correctTipoCampoVariable : boolean = true;
  correctTipoCalculadora : boolean = true;
  correctLongitudVariable  : boolean = true;
  correctDescripcionAlerta : boolean = true;
  correctEncuestaIPSVariable : boolean = true;
  correctVariableCondicionada : boolean = true;
  correctEntreRangosVariable : boolean = true;
  correctListaVariable : boolean = true;
  estado:any;
  desabilitarInput:boolean = false;
  
  constructor(private router: Router,
    private coockie: CookieService,
    private itemService: ConfigService,
    public dialog: MatDialog,
    private serviceVariableDetails: VariableDetailsService) { 

      this.userEncoded = sessionStorage.getItem("objUser");
      this.objUser = JSON.parse(this.userEncoded);

  }

  ngOnInit(): void {
    this.estado = sessionStorage.getItem("estado");
    this.detalleEncoded = sessionStorage.getItem("detalle");
    this.detalleUncoded = JSON.parse(this.detalleEncoded);
    this.idMedicion = this.detalleUncoded.idMedicion;    
    this.nombreModuloAnterior = JSON.parse(
      sessionStorage.getItem("returnModel")!
    );

    this.validarEstado();


    this.modeloVariable = sessionStorage.getItem("modelo"); 
    this.variableSelected = JSON.parse(this.modeloVariable);  
    this.variableSelected.modifyBy =  this.objUser.userId;

    if(this.variableSelected.id == 0)
    {
      this.variableSelected.createdBy =  this.objUser.userId;
    }

    //Request Inicial
    this.searchCatalogCobertura(this.variableSelected.tablaReferencial);
    this.getGrupo();
    this.getDefaultValue();
    this.getCatalogo();
    this.getItemsVariableRegla();
    this.getTipoVariable();
    this.getCalificaciones();
    this.getItemsTipoCampo();
    this.getItemsCalculadoraCampo();
    this.consultaVariablesCondicionadas();

  }

  validarEstado(){
    if(this.estado)
    this.estado!=EstadosBolsa.Creada?this.desabilitarInput=true:this.desabilitarInput=false;
  }

  //Redirect breadcrumb
  goPlaces(number: number) {
    if (number == 1) {
      this.router.navigateByUrl("/gestion-de-auditoria");
    } else {
      var encoded = btoa(JSON.stringify(this.detalleUncoded));
      this.router.navigateByUrl(
        "/gestion-de-auditoria/detalle-variable/" + encoded
      );
    }
  }

  //Tipo Varaible
  getTipoVariable() {
    this.serviceVariableDetails.GetTipo().subscribe((Response) => {
      this.tipoVariable = Response;

     if(this.variableSelected.variable == 0)
      {
        this.tipoVariable = this.tipoVariable.filter(x => x.id != enumTipoVariable.Resolucion);
      }
      
      this.ValidationLoading();
    },
    (error) => {
    
    });
  }

  //Grpo Variable
  getGrupo() {
    this.serviceVariableDetails.GetGrupo().subscribe((Response) => {
      this.grupoValueComplete = Response;
      this.grupoValue = Response;
      this.ValidationLoading();
    },
    (error) => {

    });
  }

  getDefaultValue() {
    this.serviceVariableDetails.GetDefaultValue().subscribe((Response) => {
      this.defaultValue = Response;
      this.ValidationLoading();
    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  getCalificaciones() {
    this.serviceVariableDetails.GetCalificaciones().subscribe((Response) => {
      this.calificaciones = Response;
      this.ValidationLoading();
    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  getCatalogo() {
    this.serviceVariableDetails.getCatalogo().subscribe((Response) => {
      this.catalogo = Response;
      this.ValidationLoading();
    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  getItemsVariableRegla() {
    this.itemService.getItemById(10).subscribe(
      (Response) => {
        this.reglasVariable = Response;
        this.ValidationLoading();
      },
      (error) => {
        this.openDialogLoading(false);
        this.swalError();
      }
    );
  }



  getItemsTipoCampo() {
    this.itemService.getItemById(14).subscribe((Response) => {
      this.tipoCampoList  = Response;
      this.ValidationLoading();

    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  getItemsCalculadoraCampo() {
    this.itemService.getItemById(21).subscribe((Response) => {
      this.tipoCalculadoraList  = Response;
      this.ValidationLoading();

    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }
  


  openDialogLoading(loading: boolean): void {
    if (loading) {
      this.dialog.open(LoadingComponent, {
        //width: '300px',
        disableClose: true,
        data: {},
      });
    } else {
      this.dialog.closeAll();
    }
  }
  

  ValidationLoading()
  {
    if(
      this.tipoVariable.length > 0 &&
      this.grupoValue.length > 0 &&
      this.defaultValue.length > 0 &&
      this.calificaciones.length > 0 &&
      this.catalogo.length > 0 &&
      this.condicionVariable.length > 0 &&
      this.reglasVariable.length > 0 &&
      this.tipoCampoList.length > 0 
    )
    {
      this.loading = false;
    }
    else
    {
      this.loading = true;
    }
  }

  swalError() {
    Swal.fire({
      title: "Error",
      text: "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente.",
      icon: "error",
    });
  }

  
  
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  //Tipo Varaible
  searchCatalogCobertura(searchValue: string) {

    this.objFiltro.id = '';
    this.objFiltro.activo = 'true';
    this.objFiltro.nombreCatalogo = searchValue;
    this.objFiltro.maxRows = 5;
    this.objFiltro.pageNumber = 0;
    
    this.itemService.getFiltroCatalogo(this.objFiltro).subscribe((Response) => {
      this.resultCatalogList = Response;
      this.options= Response.data;
    },
    (error) => {
    
    });
  }

  openModalVariablesCondicionada()
  {
    this.objCondicional = {
      idMedicion: this.variableSelected.medicionId,
      idVariable: this.variableSelected.variable,
      valorConstante: this.variableSelected.valorConstante,
      variableCondicionadaList: this.variableCondicionadaList
    }

    const dialogRef = this.dialog.open(VariableCondicionadaComponent, {
      data: this.objCondicional,
      disableClose: false,
      minWidth: '900px',
      width: 'auto'
    });
      dialogRef.afterClosed().subscribe(result => {
        this.variableCondicionadaList = this.serviceVariableDetails.getVariablesCondicional();
        this.variableSelected.variableCondicional = this.variableCondicionadaList;
        this.variableSelected.valorConstante = this.serviceVariableDetails.getValorConstante();
        this.BuildStringVariablesCondicionadas();
      });
  }

  consultaVariablesCondicionadas() {
    this.serviceVariableDetails.consultaVariablesCondicionadas(this.variableSelected.variable, this.idMedicion).subscribe(
        (Response) => {         
          this.variableCondicionadaList = Response;  
          this.variableSelected.variableCondicional =  this.variableCondicionadaList;
          this.BuildStringVariablesCondicionadas();    
        },
        (error) => {
          console.log(error);
        }
      );
  }

  resetVariableCondicionada(){
    if(!this.variableSelected.condicionada){
        this.textoVariablesCondicionada='';
        this.variableCondicionadaList=[];
        this.variableSelected.variableCondicional = this.variableCondicionadaList;
        this.variableSelected.valorConstante='';
    }
  }

  BuildStringVariablesCondicionadas()
  {
    this.textoVariablesCondicionada = "";

    this.variableCondicionadaList.forEach(element => {
        this.textoVariablesCondicionada += element.nombre + ", "
    });
  }


  openDialogSave() {
    if (this.validationForm()) {     
      
      const dialogRef = this.dialog.open(ModalVariableComponent, {
        data: this.variableSelected,
      });
      dialogRef.afterClosed().subscribe((result) => {});
    }
  }


  cambioTipo(){
    console.log(this.variableSelected.tipoVariableItem);
    if(this.variableSelected.tipoVariableItem==36){ //Activa check nc y nd para variable tipo informativa
      this.variableSelected.enableNC=true;
      this.variableSelected.enableND=true;
    }else if(this.variableSelected.tipoVariableItem==37){
      this.variableSelected.enableNC=true;

    }
  }

  
  variableVisible(visible:any)
  {
    ;
    if(!visible)
    {
      this.variableSelected.esCalificable = false;
    }
  }



  tipoVariableChange(id:number)
  {
    if(id == this._enumTipoVariable.Glosa)
    {
      this.grupoValue = this.grupoValueComplete;
      this.variableSelected.subGrupoId = enumGrupoVariable.Glosas;
    }
    else
    {
      this.variableSelected.subGrupoId = 0
      this.grupoValue = this.grupoValueComplete.filter(x => x.id != enumGrupoVariable.Glosas);
    }
  }


  //-------------------------------- VALIDACIONES FORMULARIO --------------------------------

  validationForm()
  {

      this.validacionTipoVariable(); 
      this.validacionNombreVariable(); 
      this.validacionOrdenVariable();
      this.validacionCalificacionDefaultVariable();
      this.validacionGrupoVariable();
      this.validacionDescripcionVariable();
      this.validacionTipoCampoVariable();
      this.validacionLongitudVariable();
      this.validacionDescripcionAlerta(); 
      this.validacionEncuestaIPSVariable(); 
      this.validacionVariableCondicionada();
      this.validacionEntreRangosVariable();
      this.validacionTipoCampoCalculadora();

      if(!this.correctTipoVariable ||
        !this.correctNombreVariable ||
        !this.correctOrdenVariable ||
        !this.correctCalificacionDefaultVariable ||
        !this.correctGrupoVariable ||
        !this.correctDescripcionVariable ||
        !this.correctTipoCampoVariable ||
        !this.correctTipoCalculadora ||
        !this.correctLongitudVariable  ||
        !this.correctDescripcionAlerta ||
        !this.correctEncuestaIPSVariable ||
        !this.correctVariableCondicionada ||
        !this.correctEntreRangosVariable ||
        !this.correctListaVariable)
      {
        this.openMessage(this._messageString.ObligatoryForm, 'warning', '');
        return false;

      }
      else
      {
        return true;
      }
    
  }

  //Open message
  openMessage(message: string, type: any, title: string)
  {
    Swal.fire({
      title: title,
      text: message,
      icon: type
    }) 
  }


  validacionTipoVariable()
  {
    if(this.variableSelected.tipoVariableItem == enumTipoVariable.Informativa)
    {
      this.variableSelected.esCalificable = false;
    }

    if(this.variableSelected.tipoVariableItem == 0)
    {
      this.correctTipoVariable = false;
    }
    else
    {
      this.correctTipoVariable = true;
    } 
  }

  validacionNombreVariable()
  {
    if((this.variableSelected.nombre == "" || this.variableSelected.nombre == null) )
    {
      this.correctNombreVariable = false;
    }
    else
    {
      this.correctNombreVariable =  true;
    } 
  }

  validacionOrdenVariable()
  {
    if(this.variableSelected.orden == 0)
    {
      this.correctOrdenVariable = false;
    }
    else
    {
      this.correctOrdenVariable = true;
    } 
  }

  validacionCalificacionDefaultVariable()
  {
    if(this.variableSelected.calificacionXDefecto == 0)
    {
      this.correctCalificacionDefaultVariable = false;
    }
    else
    {
      this.correctCalificacionDefaultVariable = true;
    } 
  }

  validacionGrupoVariable()
  {
    if(this.variableSelected.subGrupoId == 0)
    {
      this.correctGrupoVariable = false;
    }
    else
    {
      this.correctGrupoVariable = true;
    } 
  }


  validacionDescripcionVariable()
  {
    if((this.variableSelected.descripcion == "" || this.variableSelected.descripcion == null) )
    {
      this.correctDescripcionVariable = false;
    }
    else
    {
      this.correctDescripcionVariable = true;
    } 
  }

  validacionTipoCampoVariable()
  {
    if(this.variableSelected.tipoCampo != this._enumTipoCampo.Decimal)
    {
      this.variableSelected.decimales = 0;
    }

    if(this.variableSelected.tipoCampo == this._enumTipoCampo.Fecha)
    {
      this.variableSelected.longitud = 8;
    }

    if(this.variableSelected.tipoCampo == this._enumTipoCampo.Alfanumerico)
    {
      this.variableSelected.desde = "";
      this.variableSelected.hasta = "";
      this.variableSelected.validarEntreRangos = false;
    }
    

    if(this.variableSelected.tipoCampo == 0)
    {
      this.correctTipoCampoVariable = false;
    }
    else
    {
      this.correctTipoCampoVariable = true;
    } 
  }

  validacionTipoCampoCalculadora()
  {
    if(this.variableSelected.calculadora)
    {
      if(this.variableSelected.tipoCalculadora == 0)
      {
        this.correctTipoCalculadora = false;
      }
      else
      {
        this.correctTipoCalculadora = true;
      } 
    }else{
      this.variableSelected.tipoCalculadora = 0;
      this.correctTipoCalculadora = true;
    }

  
  }

  validacionLongitudVariable()
  { 
    if(this.variableSelected.lista){
      this.correctLongitudVariable = true;
    }else{
      if(this.variableSelected.longitud == 0)
      {
        this.correctLongitudVariable = false;
      }
      else
      {
        this.correctLongitudVariable = true;
      } 
    }
    
  }

  validacionDescripcionAlerta()
  {
    if(!this.variableSelected.alerta)
    {
      this.variableSelected.alertaDescripcion = "";
    }


    if(this.variableSelected.alerta && 
      (this.variableSelected.alertaDescripcion == null || this.variableSelected.alertaDescripcion == ""))
    {
      this.correctDescripcionAlerta = false;
    }
    else
    {
      this.correctDescripcionAlerta = true;
    } 
  }


  validacionEncuestaIPSVariable()
  {
    if(!this.variableSelected.encuesta)
    {
        this.variableSelected.calificacionIPSItem = [];
    }


    if(this.variableSelected.encuesta &&
       this.variableSelected.calificacionIPSItem.length == 0)
      {
        this.correctEncuestaIPSVariable = false;
      }
      else
      {
        this.correctEncuestaIPSVariable = true;
      }
   }


  validacionVariableCondicionada()
  {
    if(!this.variableSelected.condicionada)
    {
        this.variableSelected.variableCondicional = [];
        this.variableSelected.valorConstante = "";
    }


    if(this.variableSelected.condicionada &&
       (this.variableSelected.variableCondicional.length == 0 || 
        this.variableSelected.valorConstante == "" || 
        this.variableSelected.valorConstante == null))
      {
        this.correctVariableCondicionada = false;
      }
      else
      {
        this.correctVariableCondicionada = true;
      }
   }
   
   validacionEntreRangosVariable()
   {
     if(this.variableSelected.validarEntreRangos &&
        (this.variableSelected.desde == "" ||  this.variableSelected.hasta == "" ||
        this.variableSelected.desde == null ||  this.variableSelected.hasta == null    
          )
          )
     {
      this.correctEntreRangosVariable = false;
      }
      else
      {
        this.correctEntreRangosVariable = true;
      }
   }

   validacionListaVariable()
   {
     if(!this.variableSelected.lista)
     {
      this.variableSelected.tablaReferencial = "";  
     }


     if(this.variableSelected.lista &&
        (this.variableSelected.tablaReferencial == "" ||  this.variableSelected.tablaReferencial == null)
          )
     {
      this.correctListaVariable = false;
      }
      else
      {
        this.correctListaVariable = true;
      }
   }


  //-------------------------------- FIN VALIDACIONES FORMULARIO --------------------------------
  


}
