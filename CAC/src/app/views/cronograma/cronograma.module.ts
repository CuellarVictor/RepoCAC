import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CronogramaRoutingModule } from './cronograma-routing.module';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { ListCronogramaComponent } from './list-cronograma/list-cronograma.component';
import { AuditorTraceabilityComponent } from './traceability/auditor-traceability/auditor-traceability.component';
import {MatCardModule} from '@angular/material/card';
import {MatTreeModule} from '@angular/material/tree';
import {MatSelectModule} from '@angular/material/select';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatMenuModule} from '@angular/material/menu';
import {MatTableModule} from '@angular/material/table';
import {MatRadioModule} from '@angular/material/radio';
import {MatNativeDateModule} from '@angular/material/core';
import { MatSortModule } from '@angular/material/sort';
import { MatDialogModule } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';

import { GenericsModule } from '../generics/generics.module';
import { TabsModule } from '../tabs/tabs.module';
import { RegistrationOpeningComponent } from './registration-opening/registration-opening.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AuditManagementModule } from '../audit-management/audit-management.module';
import { SafePipe } from './safe.pipe';


@NgModule({
  declarations: [
    ListCronogramaComponent,
    AuditorTraceabilityComponent,
    RegistrationOpeningComponent,
    SafePipe,
       
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CronogramaRoutingModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatTooltipModule,
    MatButtonModule,
    MatIconModule,
    MatExpansionModule,
    MatInputModule,
    MatFormFieldModule,
    MatCardModule,
    MatTreeModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatMenuModule,
    MatTableModule,
    MatRadioModule,
    MatNativeDateModule,
    MatSortModule,
    MatDialogModule,
    MatPaginatorModule,
    GenericsModule,    
    AuditManagementModule,
    TabsModule,
    MatToolbarModule
  ]
})
export class CronogramaModule { }
