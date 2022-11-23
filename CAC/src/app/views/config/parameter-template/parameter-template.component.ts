import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { ParametroTemplate } from 'src/app/model/actas/parametroTemplate';
import { enumCatalog, enumTipoInputActas, messageString } from 'src/app/model/Util/enumerations.enum';
import Swal from 'sweetalert2';
import { VariableDetailsService } from '../../audit-management/variable-details/services/variable-details.service';
import { ConfigService } from '../service/config.service';
import { ModalParameterComponent } from './modal-parameter/modal-parameter.component';

@Component({
  selector: 'app-parameter-template',
  templateUrl: './parameter-template.component.html',
  styleUrls: ['./parameter-template.component.scss']
})
export class ParameterTemplateComponent implements OnInit {

  // Actas
  TipoActas: any;
  _idActa: number = 0;
  listParameterTemplate: ParametroTemplate[] = [];
  parameterSelected: ParametroTemplate = new ParametroTemplate();

   //Enumerations
   _enumCatalog: any = enumCatalog;
   _enumTipoInputActas: any = enumTipoInputActas;
   _messageString: messageString = new messageString();
   

   dataSource2!: any[];
   displayedColumns2: string[] = ['menu','Descripcion','Valor'];

  constructor(private serviceVariableDetails: VariableDetailsService,
    private configService : ConfigService,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this.getTipoActa();
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


  consultaParametrosTemplate(idTemplate: number)
  {
    this.openDialogLoading(true);
    this.configService.consultaParametrosTemplate().subscribe
    (
      (response) => {
        this.listParameterTemplate = response;

        this.listParameterTemplate = this.listParameterTemplate.filter((x:any) => x.template == idTemplate 
                                                                                      && x.tipoInput == this._enumTipoInputActas.Parametrizacion); 
         
        this.dataSource2 = this.listParameterTemplate;                                                                   
        console.log(this.listParameterTemplate)
        this.openDialogLoading(false);
      },
      (error) => { 
        this.openDialogLoading(false); 
      }
    );
  }

  openModalTemplate(elementSelected: any)
  {
    const dialogRef = this.dialog.open(ModalParameterComponent, {
      data: elementSelected,
      disableClose: false 
    });

    dialogRef.afterClosed().subscribe(result => {
      this.consultaParametrosTemplate(this._idActa);         
  });
  }


}
