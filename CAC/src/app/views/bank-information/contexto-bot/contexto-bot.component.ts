import { Component, Inject, OnInit, ViewChild } from '@angular/core';

import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ContextoModel } from 'src/app/model/bot/contextobot.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-contexto-bot',
  templateUrl: './contexto-bot.component.html',
  styleUrls: ['./contexto-bot.component.scss']
})
export class ContextoBotComponent implements OnInit {

  inputdata :ContextoModel;
  urlFiles: string = environment.urlFiles;
  
  constructor(@Inject(MAT_DIALOG_DATA) public data:any,
  public _dialogRef: MatDialogRef<ContextoBotComponent>,) {
    this.inputdata = data;
    
   }

  ngOnInit(): void {
  }

  close() {
    this._dialogRef.close();
  }

}
