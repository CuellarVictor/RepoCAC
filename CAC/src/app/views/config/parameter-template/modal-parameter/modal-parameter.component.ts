import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as moment from 'moment';
import { CookieService } from 'ngx-cookie-service';
import { RegistroAuditoriaSeguimientoModel } from 'src/app/model/registroauditoria/registroauditoriaseguimiento.model';
import { messageString } from 'src/app/model/Util/enumerations.enum';
import { ObservationsComponent } from 'src/app/views/bank-information/observations/observations.component';
import Swal from 'sweetalert2';
import { ConfigService } from '../../service/config.service';

@Component({
  selector: 'app-modal-parameter',
  templateUrl: './modal-parameter.component.html',
  styleUrls: ['./modal-parameter.component.scss']
})
export class ModalParameterComponent implements OnInit {

  objUser:any;

  constructor(@Inject(MAT_DIALOG_DATA) public data:any,
  private coockie: CookieService,
  private configService : ConfigService,
  public dialogRef: MatDialogRef<ModalParameterComponent>,) {
    console.log(data);

    this.objUser = JSON.parse(atob(this.coockie.get('objUser')));  
   }

  ngOnInit(): void {
  }


  save()
  {
    this.data.modifyBy = this.objUser.userId
    this.configService.upsertParametroTemplate(this.data).subscribe
    (
      (response) => {
        this.dialogRef.close();
        
      },
      (error) => { 
      }
    );
  }
}
