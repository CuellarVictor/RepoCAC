import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TabsService } from '../services/tabs.service';

@Component({
  selector: 'app-info',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.scss']
})
export class InfoComponent implements OnInit {
  filter!: FormGroup;
  objData:any;

  constructor(private service: TabsService,private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.filter = this.formBuilder.group({
      word: ['', [Validators.required]],
    });
  }


  getData(){
  const palabraClave = {
    palabraClave: this.filter.controls.word.value
  }
    this.service.postDataBankInformation(palabraClave).subscribe(Response => {   
      this.objData = Response;      
    })
  }


}
