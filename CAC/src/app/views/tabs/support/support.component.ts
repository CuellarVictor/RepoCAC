import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { TabsService } from '../services/tabs.service';

@Component({
  selector: 'app-support',
  templateUrl: './support.component.html',
  styleUrls: ['./support.component.scss']
})
export class SupportComponent implements OnInit {

  objAuditar:any;

  soportes:any;

  constructor(private service: TabsService,
              private coockie: CookieService) {
                this.objAuditar = JSON.parse(atob(this.coockie.get('objAuditar')));
               }

  ngOnInit(): void {
    this.getDataSoportes();
  }

  getDataSoportes(){
      this.service.GetDataSupports(this.objAuditar.id).subscribe(Response => {
        this.soportes = Response;
      })
  }

}
