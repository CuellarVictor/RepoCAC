import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagementComponent } from './management/management.component';
import { VariableComponent } from './variable/variable.component';
import { AuditManagementRoutingModule } from './audit-management-routing.module';
import { GenericsModule } from '../generics/generics.module';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatIconModule} from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MedicionComponent } from './medicion/medicion.component';
import { VariableDetailsComponent } from './variable-details/variable-details.component';
import { VariableSegmentationComponent } from './variable-segmentation/variable-segmentation.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FileUploadModule } from 'ng2-file-upload';
import { DialogComponent } from './dialog/dialog.component';
import { MatCardModule } from '@angular/material/card';
import { AsignarCronogramaComponent } from './asignar-cronograma/asignar-cronograma.component';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatRadioModule} from '@angular/material/radio';
import { ModalVariableComponent } from './modal-variable/modal-variable.component';
import { MatNativeDateModule } from '@angular/material/core';
import { ModalCarguePoblacionComponent } from './modal-cargue-poblacion/modal-cargue-poblacion.component';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {RoundProgressModule} from 'angular-svg-round-progressbar';
import { LoadingProgressComponent } from 'src/app/layout/loading/loading-progress/loading-progress.component';
import { AsignacionBolsaComponent } from './asignacion-bolsa/asignacion-bolsa.component';
import { ReasignarBolsaComponent } from './reasignar-bolsa/reasignar-bolsa.component';
import { ModalReasignarComponent } from './modal-reasignar/modal-reasignar.component';
import { ModalReasignarDescargarComponent } from './modal-reasignar-descargar/modal-reasignar-descargar.component';
import { SafePipe } from './safe.pipe';
import { ModalMoverBolsaComponent } from './modal-mover-bolsa/modal-mover-bolsa.component';
import { VariableUpsertComponent } from './variable/variable-upsert/variable-upsert/variable-upsert.component';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import { VariableCondicionadaComponent } from './variable/variable-upsert/variable-condicionada/variable-condicionada/variable-condicionada.component';
import { NgxMaskModule } from 'ngx-mask';
import { ModalEliminarRegistroAuditoriaComponent } from './modal-eliminar-registro-auditoria/modal-eliminar-registro-auditoria.component';
import { ModalCalificacionMasivaComponent } from './modal-calificacion-masiva/modal-calificacion-masiva.component';


@NgModule({
  declarations: [
    ManagementComponent,
    VariableComponent,
    MedicionComponent,
    VariableDetailsComponent,
    VariableSegmentationComponent,
    DialogComponent,
    ModalVariableComponent,
    AsignarCronogramaComponent,
    ModalCarguePoblacionComponent,
    LoadingProgressComponent,
    AsignacionBolsaComponent,
    ReasignarBolsaComponent,
    ModalReasignarComponent,
    ModalReasignarDescargarComponent,
    ModalMoverBolsaComponent,
    SafePipe,
    VariableUpsertComponent,
    VariableCondicionadaComponent,
    ModalEliminarRegistroAuditoriaComponent,
    ModalCalificacionMasivaComponent
  ],
  exports:[
    ManagementComponent
  ],
  imports: [
    CommonModule,
    AuditManagementRoutingModule,
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
    MatNativeDateModule, 
    FormsModule ,
    MatProgressSpinnerModule,
    RoundProgressModule,
    MatAutocompleteModule,
    NgxMaskModule.forRoot(),
  ],
  entryComponents: [DialogComponent,ModalVariableComponent]
})
export class AuditManagementModule { }
