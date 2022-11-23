import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileComponent } from './profile/profile.component';
import { GenericsModule } from '../generics/generics.module';
import { MatIconModule } from '@angular/material/icon';
import { UsuarioComponent } from './usuario/usuario.component';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatMenuModule } from '@angular/material/menu';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FileUploadModule } from 'ng2-file-upload';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import {MatExpansionModule} from '@angular/material/expansion';
import { NgxMaskModule } from 'ngx-mask';


@NgModule({
  declarations: [
    ProfileComponent,
    UsuarioComponent
  ],
  imports: [
    CommonModule,
    ProfileRoutingModule,
    GenericsModule,
    CommonModule,
    MatButtonModule,
    MatTableModule,
    MatSortModule,
    MatMenuModule,
    MatCheckboxModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    DragDropModule,
    ReactiveFormsModule,
    FileUploadModule,
    MatDialogModule,
    MatCardModule,
    MatDatepickerModule,
    MatRadioModule,
    FormsModule,
    MatExpansionModule,
    MatIconModule,
    NgxMaskModule.forRoot()
  ],
})
export class ProfileModule { }
