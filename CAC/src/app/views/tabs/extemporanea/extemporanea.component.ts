import { Component, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';


const URL = 'http://localhost:3000/fileupload/';

@Component({
  selector: 'app-extemporanea',
  templateUrl: './extemporanea.component.html',
  styleUrls: ['./extemporanea.component.scss']
})
export class ExtemporaneaComponent implements OnInit {

  constructor() { }
  public uploader: FileUploader = new FileUploader({ url: URL, itemAlias: 'photo' });
  ngOnInit(): void {

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
         console.log('ImageUpload:uploaded:', item, status, response);
         alert('File uploaded successfully');
    };


  }

}
