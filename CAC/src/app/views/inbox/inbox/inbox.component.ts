import { Component, OnInit } from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import { NewInboxComponent } from '../new-inbox/new-inbox.component';
@Component({
  selector: 'app-inbox',
  templateUrl: './inbox.component.html',
  styleUrls: ['./inbox.component.scss']
})
export class InboxComponent implements OnInit {

  tab1 = true;
  tab2 = false;

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  selectTab( tab:number ){
    if( tab === 1 ){
      this.tab1 = true;
      this.tab2 = false;
    }else{
      this.tab1 = false;
      this.tab2 = true;
    }
  }
  openDialog() {
    const dialogRef = this.dialog.open(NewInboxComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

}
