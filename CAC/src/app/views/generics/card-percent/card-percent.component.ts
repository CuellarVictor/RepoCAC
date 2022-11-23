import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CookieService } from 'ngx-cookie-service';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { objuser } from 'src/utils/models/cronograma/cronograma';
import Swal from 'sweetalert2';
import { genericService } from '../services/generics.service';

@Component({
  selector: 'app-card-percent',
  templateUrl: './card-percent.component.html',
  styleUrls: ['./card-percent.component.scss']
})
export class CardPercentComponent implements OnInit {
  @Input () title: string = '';
  @Input () page: string = '';
  @Input () subtitle: string = '';
  @Input () progresoDia: number = 0;
  @Input () totales: number = 0;
  @Input () user: number = 0;

  objUser: objuser;
  alerts: any;

  userCode: any;
  userEncoded: any;

  constructor(private service: genericService, 
              public dialog: MatDialog,
              private coockie: CookieService) {

                this.userEncoded = sessionStorage.getItem("objUser");
                this.objUser = JSON.parse(this.userEncoded);
              }

  ngOnInit(): void {
    this.getAlerts();
    this.showUserCode(); 
  }

  getAlerts(){
    this.service.postDate(this.objUser.userId).subscribe(Response => {
      this.alerts = Response;  
    },Error => {
      console.log(Error);
    })
  }  

  showUserCode(){  

    let code = this.objUser.codigo;

    if(code != undefined && code != null || code != "")
    {
      this.userCode = code;
    }


    
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

}
