import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfiguracionComponent } from './configuracion/configuracion.component';
import { ConfigRoutingModule } from './config-routing.module';
import { GenericsModule } from '../generics/generics.module';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
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
import { CatalogoComponent } from './catalogo/catalogo.component';
import { ListaCatalogoComponent } from './lista-catalogo/lista-catalogo.component';
import { ListaItemsComponent } from './lista-items/lista-items.component';
import { CodigosComponent } from './codigos/codigos.component';
import { SafeePipe } from './safee.pipe';
import { PermisosComponent } from './permisos/permisos.component';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { ImgPermisoModalComponent } from './img-permiso-modal/img-permiso-modal.component';
import { AdminRolComponent } from './admin-rol/admin-rol.component';
import { LiderEntidadComponent } from './lider-entidad/lider-entidad.component';
import { LiderEntidadListComponent } from './lider-entidad/lider-entidad-list/lider-entidad-list.component';
import { GenerarActasComponent } from './generar-actas/generar-actas.component';
import { ParameterTemplateComponent } from './parameter-template/parameter-template.component';
import { ModalParameterComponent } from './parameter-template/modal-parameter/modal-parameter.component';


@NgModule({
  declarations: [
    ConfiguracionComponent,
    CatalogoComponent,
    ListaCatalogoComponent,
    ListaItemsComponent,
    CodigosComponent,
    SafeePipe,
    PermisosComponent,
    ImgPermisoModalComponent,
    AdminRolComponent,
    LiderEntidadComponent,
    LiderEntidadListComponent,
    GenerarActasComponent,
    ParameterTemplateComponent,
    ModalParameterComponent
  ],
  imports: [
    CommonModule,
    ConfigRoutingModule,
    GenericsModule,
    MatButtonModule,
    MatTableModule,
    MatSortModule,
    MatIconModule,
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
    MatSlideToggleModule
  ]
})
export class ConfigModule { }
