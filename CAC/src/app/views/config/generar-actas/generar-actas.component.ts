import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { EPSItem } from 'src/app/model/actas/epsItem';
import { GenerarActaInput, Parametro } from 'src/app/model/actas/generarActaInput';
import { ParametroTemplate } from 'src/app/model/actas/parametroTemplate';
import { TipoActas } from 'src/app/model/actas/tipoActas';
import { enumCatalog, enumTipoDatoActas, enumTipoInputActas, messageString } from 'src/app/model/Util/enumerations.enum';
import Swal from 'sweetalert2';
import { AdministradorService } from '../../admin/administrador/services/administrador.service';
import { VariableDetailsService } from '../../audit-management/variable-details/services/variable-details.service';
import { ConfigService } from '../service/config.service';

@Component({
  selector: 'app-generar-actas',
  templateUrl: './generar-actas.component.html',
  styleUrls: ['./generar-actas.component.scss']
})
export class GenerarActasComponent implements OnInit
{
  // Actas
  TipoActas: any;
  _idActa: number = 0;
  _idCobertura: number = 0;
  _idEPS: string = "";

  listCobertura: any = [];
  listEPS : EPSItem [] = [];
  isCoberturaSelected : boolean = false;
  listParameterTemplate: ParametroTemplate[] = [];
  parameterTemplateTextType: ParametroTemplate[] = [];
  parameterTemplateTypeList: ParametroTemplate[] = [];

  parametricParams : ParametroTemplate[] = [];
  queriedParamsList : ParametroTemplate[] = [];
  generarActaInput : GenerarActaInput = new GenerarActaInput();

  //Enumerations
  _enumTipoInputActas: any = enumTipoInputActas;
  _enumTipoDatoActas: any = enumTipoDatoActas;
  _enumCatalog: any = enumCatalog;
  
  _messageString: messageString = new messageString();

  isGeneratePDFEnabled : boolean = false;

  constructor(private adminService : AdministradorService,
              private configService : ConfigService,
              private serviceVariableDetails: VariableDetailsService,
              public dialog: MatDialog) { }

  ngOnInit(): void
  {
    this.getTipoActa();
    this.getEnfermedades();
  }


  getTipoActa()
  {
    this.serviceVariableDetails.getItemsByCatalogId(this._enumCatalog.Actas).subscribe((data) => {
      this.TipoActas = data;
    },
    (error) => {
      this.OpenErrorodal();
    });
  }

  getEnfermedades()
  { 
    this.adminService.getEnfermedades().subscribe
    (
      (response) => {
        this.listCobertura = response;
        this.openDialogLoading(false);
      },
      (error) => { 
        this.openDialogLoading(false); 
      }
    );
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

  selectCobertura(idCobertura : number) : void
  {
    this.configService.consultaListadoEPS(idCobertura).subscribe(response => 
    {
      this.listEPS = response;
      this.isCoberturaSelected = true;
    });
  }


  consultaParametrosTemplate(idTemplate: number)
  {
    this.openDialogLoading(true);
    this.configService.consultaParametrosTemplate().subscribe
    (
      (response) => {
        this.listParameterTemplate = response;

        this.parameterTemplateTextType = this.listParameterTemplate.filter((x:any) => x.template == idTemplate 
                                                                                      && x.tipoInput == this._enumTipoInputActas.Usuario
                                                                                      && x.tipoDato == this._enumTipoDatoActas.Texto); 

        this.parameterTemplateTypeList = this.listParameterTemplate.filter((x:any) => x.template == idTemplate 
                                                                                      && x.tipoInput == this._enumTipoInputActas.Usuario
                                                                                      && x.tipoDato == this._enumTipoDatoActas.Listado);                                                                               

        this.parametricParams = this.listParameterTemplate.filter((x:any) => x.template == idTemplate && 
                                                                             x.tipoInput == this._enumTipoInputActas.Parametrizacion);
        
        this.queriedParamsList = this.listParameterTemplate.filter((x:any) => x.template == idTemplate && 
                                                                          x.tipoInput == this._enumTipoInputActas.Consultado);
        
        this.openDialogLoading(false);
      },
      (error) => { 
        this.openDialogLoading(false); 
      }
    );
  }

  generarActa()
  {
    //Asigna valores al modelo request

    this.generarActaInput = new GenerarActaInput();
    this.generarActaInput.listaParametros = [];
    this.generarActaInput.idCobertura = this._idCobertura;
    this.generarActaInput.idTemplate = this._idActa;
    this.generarActaInput.idEPS = this._idEPS;

    //Parametros tipo texto
    this.parameterTemplateTextType.forEach((parameter: any) => {
      let currentParameter:  Parametro = new Parametro();
      currentParameter.parametroTemplateKey = parameter.parametro;
      currentParameter.value = parameter.valor;
      this.generarActaInput.listaParametros.push(currentParameter);

    });

    // Añadir parametrizados y consultados de referencia
    this.parametricParams.forEach(parameter => 
    {
      let currentParameter:  Parametro = new Parametro();
      currentParameter.parametroTemplateKey = parameter.parametro;
      currentParameter.value = '';
      this.generarActaInput.listaParametros.push(currentParameter);
    });

    this.queriedParamsList.forEach(parameter => 
    {
      let currentParameter:  Parametro = new Parametro();
      currentParameter.parametroTemplateKey = '$Listado$.' + parameter.parametro;
      currentParameter.value = '';
      this.generarActaInput.listaParametros.push(currentParameter);
    });

    //Parametros tipo Lista separados por ;
    this.parameterTemplateTypeList.forEach((parameter: any) => {
      
      
      var parameterSplit = parameter.valor.split(';');

      parameterSplit.forEach((splitedItem: any) => {
        let currentParameter:  Parametro = new Parametro();
        currentParameter.parametroTemplateKey = '$Listado$.' + parameter.parametro;
        currentParameter.value = splitedItem;
        this.generarActaInput.listaParametros.push(currentParameter);
      });      

    }); 

    this.openDialogLoading(true);
    this.configService.generarActa(this.generarActaInput).subscribe
    (
      (response) => {
        console.log(response);
        this.downloadBase64File(response.valor.toString());
        this.openDialogLoading(false);
      },
      (error) => { 
        console.log(error);
        this.openDialogLoading(false); 
        this.OpenErrorodal();
      }
    );    
    
  }

  downloadBase64File(base64String:string)
  {
    const source = `data:application/pdf;base64,${base64String}`;
    const link = document.createElement("a");
    link.href = source;
    link.download = `Acta.pdf`
    link.click();
  }

  OpenErrorodal()
  {
    Swal.fire({
      title: "Error",
      text: this._messageString.ErrorMessage,
      icon: "error",
    });
  }

  OpenSuccesModal()
  {
    Swal.fire({
      title: "Correcto",
      text: this._messageString.SuccessMessage,
      icon: "success",
    });
  }

  isFormFullFilled() : boolean
  {
    let result : boolean = this.isCoberturaSelected;
    result = result && this._idActa != null && this._idActa != 0;
    result = result && this._idEPS != null && this._idEPS != '';

    this.parameterTemplateTextType.forEach(element =>
    {
      if (element.obligatorio = true)
        result = result && element.valor != null && element.valor != '';
    });
    this.parameterTemplateTypeList.forEach(element =>
    {
      if (element.obligatorio = true)
        result = result && element.valor != null && element.valor != '';
    });

    return result;
  }

  checkForm()
  {
    this.isGeneratePDFEnabled = this.isFormFullFilled();
  }

}
