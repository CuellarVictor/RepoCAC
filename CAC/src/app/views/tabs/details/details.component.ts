import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { objuser } from 'src/utils/models/cronograma/cronograma';
import { TabsService } from '../services/tabs.service';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {

  objAuditar:any;
  objObservaciones: any;
  
  constructor(private coockie: CookieService,
              private service: TabsService) { 
              this.objAuditar = JSON.parse(atob(this.coockie.get('objAuditar')));
            }

  ngOnInit(): void {  
    this.getObservations();
  }

  getObservations(){
    this.service.postGetObservations(this.objAuditar.id).subscribe(Response => {   
      this.objObservaciones = Response;      
    })
  }


}
